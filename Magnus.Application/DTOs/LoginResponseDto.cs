namespace Magnus.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAtUtc { get; set; }
        public UsuarioResponseDto User { get; set; } = null!;
    }
}
