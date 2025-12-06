namespace Magnus.Application.DTOs;

public class OrganizadorStatsDto
{
    public int EventosOrganizados { get; set; }
    public decimal IngresosTotales { get; set; }
    public double RatingPromedio { get; set; }
    public int ClientesSatisfechos { get; set; }
    public int EventosPendientes { get; set; }
    public int EventosProximos { get; set; }
}
