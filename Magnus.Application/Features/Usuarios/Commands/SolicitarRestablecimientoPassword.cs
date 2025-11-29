global using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;
using Magnus.Application.DTOs;
using Magnus.Application.Interfaces;
using Magnus.Domain.Entities;
using Magnus.Domain.Exceptions;

namespace Magnus.Application.Features.Usuarios.Commands.SolicitarRestablecimientoPassword
{
    // Comando (Input)
    public class SolicitarRestablecimientoPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    // Handler (Lógica de Negocio)
    public class SolicitarRestablecimientoPasswordHandler : IRequestHandler<SolicitarRestablecimientoPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public SolicitarRestablecimientoPasswordHandler(IUnitOfWork unitOfWork, IEmailService emailService, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(SolicitarRestablecimientoPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Usuarios.GetByEmailAsync(request.Email); // Asumiendo este método en tu Repositorio de Usuarios
            
            // Seguridad: NO indicar si el usuario existe o no
            if (user == null) 
            {
                return true; 
            }

            // 1. Generar Token y Expiración (ej: 2 horas)
            var token = _tokenService.GeneratePasswordResetToken();
            var expiration = DateTime.UtcNow.AddHours(2);

            // 2. Guardar Token en la entidad de Dominio
            user.SetPasswordResetToken(token, expiration);
            
            // No se necesita llamar a Update en EF Core para la mayoría de las entidades rastreadas,
            // pero es bueno para la consistencia si se usa un repositorio genérico.
            _unitOfWork.Usuarios.Update(user); 
            await _unitOfWork.CommitAsync();

            // 3. Crear el Enlace de Restablecimiento (ajusta TU_URL_FRONTEND)
            var resetLink = $"TU_URL_FRONTEND/auth/reset-password?email={Uri.EscapeDataString(user.Email)}&token={token}";

            // 4. Enviar Correo
            var subject = "Restablecimiento de Contraseña - EventosMagnus";
            var body = $"Hola {user.Nombre},\n\nHas solicitado restablecer tu contraseña. Por favor, utiliza el siguiente enlace o token para cambiarla:\n\nLink: {resetLink}\nToken: {token}\n\nEste enlace expirará en 2 horas.";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return true;
        }
    }
}