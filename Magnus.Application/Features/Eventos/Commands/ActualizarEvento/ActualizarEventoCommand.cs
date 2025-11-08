namespace Magnus.Application.Features.Eventos.Commands.ActualizarEvento
{
    public class ActualizarEventoCommand
    {
        public Guid EventoId { get; }
        public string Titulo { get; }
        public string? Descripcion { get; }
        public DateTime FechaInicio { get; }
        public DateTime FechaFin { get; }
        public string Lugar { get; }
        public int Capacidad { get; }

        public ActualizarEventoCommand(Guid eventoId, string titulo, DateTime fechaInicio, 
            DateTime fechaFin, string lugar, int capacidad, string? descripcion = null)
        {
            EventoId = eventoId;
            Titulo = titulo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Lugar = lugar;
            Capacidad = capacidad;
            Descripcion = descripcion;
        }
    }
}
