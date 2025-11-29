using Magnus.Domain.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using MediatR;

namespace Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId
{
    public class ObtenerEventoPorIdQueryHandler : IRequestHandler<ObtenerEventoPorIdQuery, EventoResponseDto?>
    {
        private readonly IUnitOfWork _uow;

        public ObtenerEventoPorIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventoResponseDto?> Handle(ObtenerEventoPorIdQuery query, CancellationToken ct = default)
        {
            var evento = await _uow.Eventos.GetByIdAsync(query.EventoId);
            return evento?.ToResponseDto();
        }
    }
}
