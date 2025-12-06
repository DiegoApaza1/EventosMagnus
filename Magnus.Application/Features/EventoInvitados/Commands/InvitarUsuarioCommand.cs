using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    // ============ InvitarUsuario ============
    public record InvitarUsuarioCommand(
        Guid EventoId,
        Guid UsuarioId,
        string? Mensaje = null) 
        : IRequest<EventoInvitadoResponseDto>;
    
    public class InvitarUsuarioCommandHandler : IRequestHandler<InvitarUsuarioCommand, EventoInvitadoResponseDto>
    {
        private readonly IUnitOfWork _uow;

        public InvitarUsuarioCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventoInvitadoResponseDto> Handle(InvitarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var evento = await _uow.Eventos.GetByIdAsync(request.EventoId);
            if (evento == null)
                throw new InvalidOperationException("Evento no encontrado");

            var usuario = await _uow.Usuarios.GetByIdAsync(request.UsuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var existente = await _uow.EventoInvitados.GetByEventoAndUsuarioAsync(request.EventoId, request.UsuarioId);
            if (existente != null)
                throw new InvalidOperationException("Ya existe una invitación para este usuario en este evento");

            var invitacion = new EventoInvitado(
                request.EventoId,
                request.UsuarioId,
                esAutopostulacion: false,
                request.Mensaje);

            await _uow.EventoInvitados.AddAsync(invitacion);
            await _uow.CommitAsync();

            return new EventoInvitadoResponseDto
            {
                Id = invitacion.Id,
                EventoId = invitacion.EventoId,
                UsuarioId = invitacion.UsuarioId,
                Estado = (int)invitacion.Estado,
                EsAutopostulacion = invitacion.EsAutopostulacion,
                FechaInvitacion = invitacion.FechaInvitacion,
                FechaRespuesta = invitacion.FechaRespuesta,
                Mensaje = invitacion.Mensaje,
                Usuario = new UsuarioSimpleDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email
                }
            };
        }
    }
}
