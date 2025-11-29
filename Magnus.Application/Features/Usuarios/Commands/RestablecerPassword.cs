// Global using directives for MediatR
global using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces;
using Magnus.Domain.Entities;
using Magnus.Domain.Exceptions;

namespace Magnus.Application.Features.Usuarios.Commands.RestablecerPassword
{
    // Comando (Input)
    public class RestablecerPasswordCommand : IRequest<bool>
    {
        public RestablecerPasswordRequestDto Dto { get; }

        public RestablecerPasswordCommand(RestablecerPasswordRequestDto dto)
        {
            Dto = dto;
        }
    }

    // Handler (Lógica de Negocio)
    public class RestablecerPasswordHandler : IRequestHandler<RestablecerPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestablecerPasswordHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RestablecerPasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var user = await _unitOfWork.Usuarios.GetByEmailAsync(dto.Email);

            // Validación de existencia y token
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordResetToken) || user.ResetTokenExpires == null)
            {
                return false; 
            }

            // 1. Validar Expiración y Coincidencia de Token
            if (user.ResetTokenExpires < DateTime.UtcNow || user.PasswordResetToken != dto.Token)
            {
                // Limpiar el token incluso si falla por seguridad (token de un solo uso)
                user.ClearPasswordResetToken(); 
                await _unitOfWork.CommitAsync();
                return false; // Token inválido o expirado
            }

            // 2. Generar un nuevo hash de contraseña 
            // **IMPORTANTE: Debes usar la misma lógica de hashing y salting que usas en el registro.**
            var newPasswordHash = CreatePasswordHash(dto.NuevaPassword); 
            user.UpdatePassword(newPasswordHash);
            
            // 3. Limpiar el token
            user.ClearPasswordResetToken();

            _unitOfWork.Usuarios.Update(user);
            await _unitOfWork.CommitAsync();

            return true;
        }
        
        private string CreatePasswordHash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}