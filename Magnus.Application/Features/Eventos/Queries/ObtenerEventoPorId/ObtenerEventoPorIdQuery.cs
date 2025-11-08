namespace Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId
{
    public class ObtenerEventoPorIdQuery
    {
        public Guid EventoId { get; }

        public ObtenerEventoPorIdQuery(Guid eventoId)
        {
            EventoId = eventoId;
        }
    }
}
