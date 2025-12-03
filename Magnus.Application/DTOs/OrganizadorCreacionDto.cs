namespace Magnus.Application.DTOs
{
    public class OrganizadorCreacionDto
    {
        public string NombreEmpresa { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Telefono { get; set; } = null!;
        public string? Direccion { get; set; }
        public decimal PrecioPorEvento { get; set; }
        public int AñosExperiencia { get; set; }
        public string? Especialidad { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
