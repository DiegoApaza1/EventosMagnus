namespace Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador
{
    public class ListarEventosPorOrganizadorQuery
    {
        public Guid OrganizadorId { get; }

        public ListarEventosPorOrganizadorQuery(Guid organizadorId)
        {
            OrganizadorId = organizadorId;
        }
    }
}
