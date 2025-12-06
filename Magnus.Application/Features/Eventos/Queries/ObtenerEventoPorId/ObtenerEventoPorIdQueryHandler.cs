using AutoMapper;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Domain.Interfaces.Services;
using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId
{
    public class ObtenerEventoPorIdQueryHandler : IRequestHandler<ObtenerEventoPorIdQuery, EventoResponseDto?>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ObtenerEventoPorIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<EventoResponseDto?> Handle(ObtenerEventoPorIdQuery query, CancellationToken ct = default)
        {
            var evento = await _uow.Eventos.GetByIdAsync(query.EventoId);
            return _mapper.Map<EventoResponseDto?>(evento);
        }
    }
}
