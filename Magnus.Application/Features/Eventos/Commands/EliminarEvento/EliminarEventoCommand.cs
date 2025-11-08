namespace Magnus.Application.Features.Eventos.Commands.EliminarEvento
{
    public class EliminarEventoCommand
    {
        public Guid EventoId { get; }

        public EliminarEventoCommand(Guid eventoId)
        {
            EventoId = eventoId;
        }
    }
}
