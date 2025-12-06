namespace Magnus.Application.DTOs
{
    public class EventoInvitadoResponseDto
    {
        public Guid Id { get; set; }
        public Guid EventoId { get; set; }
        public Guid UsuarioId { get; set; }
        public int Estado { get; set; }
        public bool EsAutopostulacion { get; set; }
        public DateTime FechaInvitacion { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? Mensaje { get; set; }
        public EventoSimpleDto? Evento { get; set; }
        public UsuarioSimpleDto? Usuario { get; set; }
    }

    public class EventoSimpleDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Lugar { get; set; } = null!;
    }

    public class UsuarioSimpleDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class InvitarUsuarioDto
    {
        public Guid EventoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string? Mensaje { get; set; }
    }

    public class AutopostularseDto
    {
        public Guid EventoId { get; set; }
        public string? Mensaje { get; set; }
    }
}
