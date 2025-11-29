using Magnus.Domain.Exceptions;
using System;

namespace Magnus.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        // PROPIEDADES AGREGADAS PARA EL RESTABLECIMIENTO
        public string? PasswordResetToken { get; private set; }
        public DateTime? ResetTokenExpires { get; private set; }
        
        private Usuario() { }

        public Usuario(string nombre, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Nombre)) throw new DomainException("Nombre requerido.");
            if (string.IsNullOrWhiteSpace(Email)) throw new DomainException("Email requerido.");
        }
        
        // MÉTODOS AGREGADOS PARA EL RESTABLECIMIENTO
        public void SetPasswordResetToken(string token, DateTime expiration)
        {
            PasswordResetToken = token;
            ResetTokenExpires = expiration;
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            ResetTokenExpires = null;
        }

        // Método para actualizar la contraseña (usado en el handler de restablecimiento)
        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash)) throw new DomainException("Hash de contraseña requerido.");
            PasswordHash = newPasswordHash;
        }
    }
}