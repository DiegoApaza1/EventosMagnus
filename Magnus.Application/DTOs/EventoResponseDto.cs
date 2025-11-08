namespace Magnus.Application.DTOs
{
    /// <summary>
    /// DTO de respuesta para Evento
    /// </summary>
    public class EventoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Lugar { get; set; } = null!;
        public int Capacidad { get; set; }
        public Guid OrganizadorId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Información del organizador (opcional, para evitar consultas adicionales)
        public OrganizadorDto? Organizador { get; set; }
    }

    /// <summary>
    /// DTO anidado para información básica del organizador
    /// </summary>
    public class OrganizadorDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
