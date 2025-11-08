using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Magnus.Application.DTOs
{
    public class EventoCreacionDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
        [DefaultValue("Conferencia Tech 2025")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [DefaultValue("2025-12-01T09:00:00")]
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddDays(30);

        [Required]
        [DefaultValue("2025-12-01T18:00:00")]
        public DateTime FechaFin { get; set; } = DateTime.Now.AddDays(30).AddHours(9);

        [Required(ErrorMessage = "El lugar es obligatorio")]
        [StringLength(300, MinimumLength = 3)]
        [DefaultValue("Auditorio Principal")]
        public string Lugar { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000, ErrorMessage = "La capacidad debe estar entre 1 y 10000")]
        [DefaultValue(200)]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El organizador es obligatorio")]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid OrganizadorId { get; set; }

        [StringLength(1000)]
        [DefaultValue("Una conferencia sobre las últimas tecnologías")]
        public string? Descripcion { get; set; }
    }
}