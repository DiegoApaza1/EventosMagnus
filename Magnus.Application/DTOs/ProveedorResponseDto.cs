namespace Magnus.Application.DTOs
{
    public class ProveedorResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Servicio { get; set; }
    }
}
