using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerEstadisticasOrganizador;

public class ObtenerEstadisticasOrganizadorQueryHandler 
    : IRequestHandler<ObtenerEstadisticasOrganizadorQuery, OrganizadorStatsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public ObtenerEstadisticasOrganizadorQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizadorStatsDto> Handle(
        ObtenerEstadisticasOrganizadorQuery request, 
        CancellationToken cancellationToken)
    {
        var organizador = await _unitOfWork.Organizadores.GetByIdAsync(request.OrganizadorId);
        
        if (organizador == null)
            throw new KeyNotFoundException("Organizador no encontrado");

        // Por ahora retornamos datos simulados
        // TODO: Implementar lógica real cuando tengamos la relación Evento-Organizador
        var stats = new OrganizadorStatsDto
        {
            EventosOrganizados = 0,
            IngresosTotales = 0,
            RatingPromedio = (double)organizador.Rating,
            ClientesSatisfechos = 0,
            EventosPendientes = 0,
            EventosProximos = 0
        };

        return stats;
    }
}
