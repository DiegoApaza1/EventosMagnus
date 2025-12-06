using System.ComponentModel.DataAnnotations;

namespace Magnus.Application.DTOs
{
    public class OrganizadorCreacionDto
    {
        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombreEmpresa { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida")]
        [MinLength(50, ErrorMessage = "La descripción debe tener al menos 50 caracteres")]
        [MaxLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [MinLength(10, ErrorMessage = "El teléfono debe tener al menos 10 caracteres")]
        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es requerida")]
        [MinLength(5, ErrorMessage = "La dirección debe tener al menos 5 caracteres")]
        [MaxLength(300, ErrorMessage = "La dirección no puede exceder 300 caracteres")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "El precio por evento es requerido")]
        [Range(1000, 500000, ErrorMessage = "El precio debe estar entre $1,000 y $500,000")]
        public decimal PrecioPorEvento { get; set; }

        [Required(ErrorMessage = "Los años de experiencia son requeridos")]
        [Range(1, 50, ErrorMessage = "Los años de experiencia deben estar entre 1 y 50")]
        public int AñosExperiencia { get; set; }

        [Required(ErrorMessage = "La especialidad es requerida")]
        [MinLength(5, ErrorMessage = "La especialidad debe tener al menos 5 caracteres")]
        [MaxLength(200, ErrorMessage = "La especialidad no puede exceder 200 caracteres")]
        public string Especialidad { get; set; } = null!;

        [Required]
        public Guid UsuarioId { get; set; }
    }
}
