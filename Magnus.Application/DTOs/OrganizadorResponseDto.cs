namespace Magnus.Application.DTOs
{
    public class OrganizadorResponseDto
    {
        public Guid Id { get; set; }
        public string NombreEmpresa { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Telefono { get; set; } = null!;
        public string? Direccion { get; set; }
        public decimal PrecioPorEvento { get; set; }
        public int AñosExperiencia { get; set; }
        public string? Especialidad { get; set; }
        public bool Verificado { get; set; }
        public decimal Rating { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
