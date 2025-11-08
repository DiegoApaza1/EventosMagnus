using Magnus.Application.DTOs;

namespace Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario
{
    /// <summary>
    /// Command que representa la intención de registrar un usuario.
    /// </summary>
    public class RegistrarUsuarioCommand
    {
        public UsuarioRegistroDto Dto { get; }

        public RegistrarUsuarioCommand(UsuarioRegistroDto dto)
        {
            Dto = dto;
        }
    }
}