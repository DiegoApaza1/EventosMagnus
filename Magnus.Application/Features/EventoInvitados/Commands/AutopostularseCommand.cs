using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    public record AutopostularseCommand(
        Guid EventoId,
        Guid UsuarioId,
        string? Mensaje = null) 
        : IRequest<EventoInvitadoResponseDto>;
    
    public class AutopostularseCommandHandler : IRequestHandler<AutopostularseCommand, EventoInvitadoResponseDto>
    {
        private readonly IUnitOfWork _uow;

        public AutopostularseCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventoInvitadoResponseDto> Handle(AutopostularseCommand request, CancellationToken cancellationToken)
        {
            var evento = await _uow.Eventos.GetByIdAsync(request.EventoId);
            if (evento == null)
                throw new InvalidOperationException("Evento no encontrado");

            var usuario = await _uow.Usuarios.GetByIdAsync(request.UsuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var existente = await _uow.EventoInvitados.GetByEventoAndUsuarioAsync(request.EventoId, request.UsuarioId);
            if (existente != null)
                throw new InvalidOperationException("Ya existe una solicitud para este evento");

            var invitacion = new EventoInvitado(
                request.EventoId,
                request.UsuarioId,
                esAutopostulacion: true,
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
                Evento = new EventoSimpleDto
                {
                    Id = evento.Id,
                    Titulo = evento.Titulo,
                    Descripcion = evento.Descripcion,
                    FechaInicio = evento.FechaInicio,
                    Lugar = evento.Lugar
                }
            };
        }
    }
}
