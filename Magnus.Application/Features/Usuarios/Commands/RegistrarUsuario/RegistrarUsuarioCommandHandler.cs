using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnus.Domain.Interfaces;
using Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario;
using Magnus.Domain.Entities;
using MediatR;

namespace Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioCommandHandler : IRequestHandler<RegistrarUsuarioCommand, Usuario>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService? _emailService;

        public RegistrarUsuarioCommandHandler(IUnitOfWork uow, IEmailService? emailService = null)
        {
            _uow = uow;
            _emailService = emailService;
        }

        public async Task<Usuario> Handle(RegistrarUsuarioCommand command, CancellationToken ct)
        {
            var dto = command.Dto;

            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("Nombre requerido.");
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido.");
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password requerido.");

            var existing = await _uow.Usuarios.GetAllAsync();
            foreach (var u in existing)
            {
                if (string.Equals(u.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Ya existe un usuario con ese email.");
                }
            }

            string passwordHash = ComputeSha256Hash(dto.Password);

            var usuario = new Usuario(dto.Nombre, dto.Email, passwordHash);

            await _uow.Usuarios.AddAsync(usuario);
            await _uow.CommitAsync();
            if (_emailService != null)
            {
                var subject = "Bienvenido a Proyecto Magnus";
                var body = $"Hola {usuario.Nombre}, tu cuenta ha sido creada correctamente.";
                _ = _emailService.SendEmailAsync(usuario.Email, subject, body);
            }

            return usuario;
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
