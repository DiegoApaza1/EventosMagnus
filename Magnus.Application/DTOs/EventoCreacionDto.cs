namespace Magnus.Application.DTOs
{
    public class EventoCreacionDto
    {
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public int Capacidad { get; set; }

        // 👇 CAMBIAR AQUÍ:
        public Guid OrganizadorId { get; set; }  // Antes era int

        public string Descripcion { get; set; } = string.Empty;
    }
}