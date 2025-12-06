using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Queries
{
    public record ObtenerInvitacionesPorUsuarioQuery(Guid UsuarioId) : IRequest<IEnumerable<EventoInvitadoResponseDto>>;
    
    public class ObtenerInvitacionesPorUsuarioQueryHandler : IRequestHandler<ObtenerInvitacionesPorUsuarioQuery, IEnumerable<EventoInvitadoResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public ObtenerInvitacionesPorUsuarioQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<EventoInvitadoResponseDto>> Handle(ObtenerInvitacionesPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var invitaciones = await _uow.EventoInvitados.GetByUsuarioIdAsync(request.UsuarioId);

            return invitaciones.Select(i => new EventoInvitadoResponseDto
            {
                Id = i.Id,
                EventoId = i.EventoId,
                UsuarioId = i.UsuarioId,
                Estado = (int)i.Estado,
                EsAutopostulacion = i.EsAutopostulacion,
                FechaInvitacion = i.FechaInvitacion,
                FechaRespuesta = i.FechaRespuesta,
                Mensaje = i.Mensaje,
                Evento = i.Evento != null ? new EventoSimpleDto
                {
                    Id = i.Evento.Id,
                    Titulo = i.Evento.Titulo,
                    Descripcion = i.Evento.Descripcion,
                    FechaInicio = i.Evento.FechaInicio,
                    Lugar = i.Evento.Lugar
                } : null
            });
        }
    }
}
