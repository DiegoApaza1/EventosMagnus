using Hangfire;
using Hangfire.MemoryStorage;
using Magnus.Api.Middlewares;
using Magnus.Application.Features.Eventos.Commands.CrearEvento;
using Magnus.Application.Features.Proveedores.Queries.BuscarProveedores;
using Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario;
using Magnus.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Magnus.Application.Interfaces;
using Magnus.Infrastructure.Persistence.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar PostgreSQL con EF Core
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<MagnusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Inyección de dependencias (UnitOfWork, EmailService)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork en Magnus.Infrastructure
builder.Services.AddScoped<Magnus.Application.Interfaces.IEmailService, Magnus.Infrastructure.Services.EmailService>();

// Handlers de Usuarios
builder.Services.AddScoped<RegistrarUsuarioCommandHandler>();

// Handlers de Eventos (CQRS completo)
builder.Services.AddScoped<CrearEventoCommandHandler>();
builder.Services.AddScoped<Magnus.Application.Features.Eventos.Commands.ActualizarEvento.ActualizarEventoCommandHandler>();
builder.Services.AddScoped<Magnus.Application.Features.Eventos.Commands.EliminarEvento.EliminarEventoCommandHandler>();
builder.Services.AddScoped<Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId.ObtenerEventoPorIdQueryHandler>();
builder.Services.AddScoped<Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador.ListarEventosPorOrganizadorQueryHandler>();

// Handlers de Proveedores
builder.Services.AddScoped<BuscarProveedoresQueryHandler>();

// 🔹 Hangfire
builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

// 🔹 Middlewares y controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventosMagnus API",
        Version = "v1",
        Description = "API para la gestión de eventos"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Carga el middleware de Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventosMagnus API V1");
        c.RoutePrefix = string.Empty; // Para abrir Swagger en /
    });
}

// Middleware global de excepciones
app.UseGlobalExceptionHandling();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
}

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Exponer el programa para pruebas (las instrucciones de nivel superior crean implícitamente una clase Programa).
public partial class Program { }