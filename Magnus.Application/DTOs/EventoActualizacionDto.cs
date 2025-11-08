using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Magnus.Application.DTOs
{
    public class EventoActualizacionDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 3)]
        [DefaultValue("Conferencia Tech 2025 - Actualizado")]
        public string Titulo { get; set; } = null!;

        [StringLength(1000)]
        [DefaultValue("Descripción actualizada del evento")]
        public string? Descripcion { get; set; }

        [Required]
        [DefaultValue("2025-12-01T10:00:00")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DefaultValue("2025-12-01T19:00:00")]
        public DateTime FechaFin { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        [DefaultValue("Auditorio Principal - Sala B")]
        public string Lugar { get; set; } = null!;

        [Required]
        [Range(1, 10000)]
        [DefaultValue(250)]
        public int Capacidad { get; set; }
    }
}
