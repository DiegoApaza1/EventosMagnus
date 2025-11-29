using System.ComponentModel.DataAnnotations;

namespace Magnus.Application.DTOs
{
    public class RestablecerPasswordRequestDto
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        public string Token { get; set; }
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaPassword { get; set; }
    }
}