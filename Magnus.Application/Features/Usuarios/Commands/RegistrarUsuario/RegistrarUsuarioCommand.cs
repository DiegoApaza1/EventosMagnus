using Magnus.Application.DTOs;

namespace Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioCommand
    {
        public UsuarioRegistroDto Dto { get; }

        public RegistrarUsuarioCommand(UsuarioRegistroDto dto)
        {
            Dto = dto;
        }
    }
}