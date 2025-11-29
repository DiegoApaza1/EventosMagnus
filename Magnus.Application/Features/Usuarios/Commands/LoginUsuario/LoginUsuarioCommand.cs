using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Usuarios.Commands.LoginUsuario
{
    public class LoginUsuarioCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; }
        public string Password { get; }

        public LoginUsuarioCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
