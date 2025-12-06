using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerEstadisticasOrganizador;

public record ObtenerEstadisticasOrganizadorQuery(Guid OrganizadorId) : IRequest<OrganizadorStatsDto>;
