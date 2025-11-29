using Magnus.Domain.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador
{
    public class ListarEventosPorOrganizadorQueryHandler : IRequestHandler<ListarEventosPorOrganizadorQuery, IEnumerable<EventoResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public ListarEventosPorOrganizadorQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<EventoResponseDto>> Handle(ListarEventosPorOrganizadorQuery query, CancellationToken ct = default)
        {
            var eventos = await _uow.Eventos.GetByOrganizadorIdAsync(query.OrganizadorId);
            return eventos.ToResponseDtoList();
        }
    }
}
