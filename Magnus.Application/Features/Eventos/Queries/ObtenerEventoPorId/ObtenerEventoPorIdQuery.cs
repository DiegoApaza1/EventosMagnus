using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId
{
    public class ObtenerEventoPorIdQuery : IRequest<EventoResponseDto?>
    {
        public Guid EventoId { get; }

        public ObtenerEventoPorIdQuery(Guid eventoId)
        {
            EventoId = eventoId;
        }
    }
}
