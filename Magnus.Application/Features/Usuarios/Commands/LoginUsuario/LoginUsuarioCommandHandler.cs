using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnus.Domain.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using MediatR;

namespace Magnus.Application.Features.Usuarios.Commands.LoginUsuario
{
    public class LoginUsuarioCommandHandler : IRequestHandler<LoginUsuarioCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;

        public LoginUsuarioCommandHandler(IUnitOfWork uow, ITokenService tokenService)
        {
            _uow = uow;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> Handle(LoginUsuarioCommand command, CancellationToken ct)
        {
            // Buscar usuario por email
            var user = await _uow.Usuarios.GetByEmailAsync(command.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            var incomingHash = ComputeSha256Hash(command.Password);
            if (!string.Equals(incomingHash, user.PasswordHash, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            var token = _tokenService.GenerateToken(user.Id, user.Nombre, user.Email);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                User = user.ToResponseDto()
            };
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
