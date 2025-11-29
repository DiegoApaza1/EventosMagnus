using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador
{
    public class ListarEventosPorOrganizadorQuery : IRequest<IEnumerable<EventoResponseDto>>
    {
        public Guid OrganizadorId { get; }

        public ListarEventosPorOrganizadorQuery(Guid organizadorId)
        {
            OrganizadorId = organizadorId;
        }
    }
}
