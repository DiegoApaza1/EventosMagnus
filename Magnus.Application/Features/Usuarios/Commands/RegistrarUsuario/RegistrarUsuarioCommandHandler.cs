using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnus.Application.Interfaces;
using Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario;
using Magnus.Domain.Entities;

namespace Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario
{
    /// <summary>
    /// Handler para registrar usuarios.
    /// </summary>
    public class RegistrarUsuarioCommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService? _emailService;

        public RegistrarUsuarioCommandHandler(IUnitOfWork uow, IEmailService? emailService = null)
        {
            _uow = uow;
            _emailService = emailService;
        }

        public async Task<Usuario> Handle(RegistrarUsuarioCommand command, CancellationToken ct = default)
        {
            var dto = command.Dto;

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("Nombre requerido.");
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido.");
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password requerido.");

            // Revisa si el email ya existe
            var existing = await _uow.Usuarios.GetAllAsync();
            foreach (var u in existing)
            {
                if (string.Equals(u.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Ya existe un usuario con ese email.");
                }
            }

            // Hash simple de la contraseña (SHA256) — puedes reemplazar por BCrypt
            string passwordHash = ComputeSha256Hash(dto.Password);

            // Crear entidad de dominio
            var usuario = new Usuario(dto.Nombre, dto.Email, passwordHash);

            // Guardar
            await _uow.Usuarios.AddAsync(usuario);
            await _uow.CommitAsync();

            // Notificación opcional por email
            if (_emailService != null)
            {
                var subject = "Bienvenido a Proyecto Magnus";
                var body = $"Hola {usuario.Nombre}, tu cuenta ha sido creada correctamente.";
                // No esperamos/esperar en background para no bloquear
                _ = _emailService.SendEmailAsync(usuario.Email, subject, body);
            }

            return usuario;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
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
