using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Queries
{
    public record ObtenerInvitacionesPorEventoQuery(Guid EventoId) : IRequest<IEnumerable<EventoInvitadoResponseDto>>;
    
    public class ObtenerInvitacionesPorEventoQueryHandler : IRequestHandler<ObtenerInvitacionesPorEventoQuery, IEnumerable<EventoInvitadoResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public ObtenerInvitacionesPorEventoQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<EventoInvitadoResponseDto>> Handle(ObtenerInvitacionesPorEventoQuery request, CancellationToken cancellationToken)
        {
            var invitaciones = await _uow.EventoInvitados.GetByEventoIdAsync(request.EventoId);

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
                Usuario = i.Usuario != null ? new UsuarioSimpleDto
                {
                    Id = i.Usuario.Id,
                    Nombre = i.Usuario.Nombre,
                    Email = i.Usuario.Email
                } : null
            });
        }
    }
}
