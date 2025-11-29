namespace Magnus.Application.DTOs
{
    public class OrganizadorCreacionDto
    {
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Guid UsuarioId { get; set; }
    }
}
