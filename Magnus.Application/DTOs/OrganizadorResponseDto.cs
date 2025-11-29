namespace Magnus.Application.DTOs
{
    public class OrganizadorResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Guid UsuarioId { get; set; }
    }
}
