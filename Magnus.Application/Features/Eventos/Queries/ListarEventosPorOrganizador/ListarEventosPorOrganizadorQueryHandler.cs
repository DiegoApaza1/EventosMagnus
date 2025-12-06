using AutoMapper;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Domain.Interfaces.Services;
using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador
{
    public class ListarEventosPorOrganizadorQueryHandler : IRequestHandler<ListarEventosPorOrganizadorQuery, IEnumerable<EventoResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ListarEventosPorOrganizadorQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventoResponseDto>> Handle(ListarEventosPorOrganizadorQuery query, CancellationToken ct = default)
        {
            var eventos = await _uow.Eventos.GetByOrganizadorIdAsync(query.OrganizadorId);
            return _mapper.Map<IEnumerable<EventoResponseDto>>(eventos);
        }
    }
}
