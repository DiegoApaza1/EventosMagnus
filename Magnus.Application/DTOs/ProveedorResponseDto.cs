namespace Magnus.Application.DTOs
{
    /// <summary>
    /// DTO de respuesta para Proveedor
    /// </summary>
    public class ProveedorResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Servicio { get; set; }
    }
}
