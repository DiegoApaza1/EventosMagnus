namespace Magnus.Application.DTOs
{
    /// <summary>
    /// DTO de respuesta para Usuario
    /// </summary>
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        
        // NO incluimos PasswordHash por seguridad
    }
}
