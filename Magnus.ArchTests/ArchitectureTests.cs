using System;
using System.Linq;
using NetArchTest.Rules;
using Xunit;

namespace Magnus.ArchTests
{
    public class ArchitectureTests
    {
        // Namespaces
        private const string DomainNs = "Magnus.Domain";
        private const string ApplicationNs = "Magnus.Application";
        private const string InfrastructureNs = "Magnus.Infrastructure";
        private const string ApiNs = "Magnus.Api";

        [Fact]
        public void Domain_No_Dependencies_On_Other_Layers()
        {
            var result = Types.InAssembly(typeof(Magnus.Domain.Entities.Usuario).Assembly)
                .Should()
                .NotHaveDependencyOnAny(ApplicationNs, InfrastructureNs, ApiNs)
                .GetResult();

            var failing1 = result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>();
            Assert.True(result.IsSuccessful, string.Join(Environment.NewLine, failing1));
        }

        [Fact]
        public void Application_Depends_Only_On_Domain()
        {
            var result = Types.InAssembly(typeof(Magnus.Application.Interfaces.IUnitOfWork).Assembly)
                .Should()
                .NotHaveDependencyOnAny(InfrastructureNs, ApiNs)
                .GetResult();

            Assert.True(result.IsSuccessful, string.Join(Environment.NewLine, result.FailingTypes));
        }

        [Fact]
        public void Infrastructure_Does_Not_Depend_On_Api()
        {
            var result = Types.InAssembly(typeof(Magnus.Infrastructure.Persistence.Repositories.UnitOfWork).Assembly)
                .Should()
                .NotHaveDependencyOn(ApiNs)
                .GetResult();

            Assert.True(result.IsSuccessful, string.Join(Environment.NewLine, result.FailingTypes));
        }

        [Fact]
        public void Api_Controllers_Should_Not_Depend_On_Infrastructure()
        {
            var result = Types.InAssembly(typeof(Magnus.Api.Controllers.EventosController).Assembly)
                .That()
                .ResideInNamespace(ApiNs + ".Controllers")
                .Should()
                .NotHaveDependencyOn(InfrastructureNs)
                .GetResult();

            Assert.True(result.IsSuccessful, string.Join(Environment.NewLine, result.FailingTypes));
        }

        [Fact]
        public void There_Is_A_Global_Exception_Middleware_In_Api()
        {
            // Verificar por reflexión que existe al menos un tipo en el namespace de Middlewares
            var assembly = typeof(Magnus.Api.Middlewares.GlobalExceptionHandlingMiddleware).Assembly;
            var anyMiddleware = assembly
                .GetTypes()
                .Any(t => t.IsClass && t.Namespace == ApiNs + ".Middlewares");

            Assert.True(anyMiddleware, "No se encontró un middleware en el namespace Magnus.Api.Middlewares. Crea por ejemplo GlobalExceptionHandlingMiddleware.");
        }

        [Fact]
        public void UnitOfWork_Is_Implemented_In_Infrastructure()
        {
            // Convention: IUnitOfWork is implemented in Infrastructure
            var result = Types.InAssembly(typeof(Magnus.Infrastructure.Persistence.Repositories.UnitOfWork).Assembly)
                .That()
                .ImplementInterface(typeof(Magnus.Application.Interfaces.IUnitOfWork))
                .Should()
                .ResideInNamespace(InfrastructureNs)
                .GetResult();

            Assert.True(result.IsSuccessful, string.Join(Environment.NewLine, result.FailingTypes));
        }
    }
}
