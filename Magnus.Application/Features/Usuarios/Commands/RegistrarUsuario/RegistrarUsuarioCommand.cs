using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using MediatR;

namespace Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario
{
    public class RegistrarUsuarioCommand : IRequest<Usuario>
    {
        public UsuarioRegistroDto Dto { get; }

        public RegistrarUsuarioCommand(UsuarioRegistroDto dto)
        {
            Dto = dto;
        }
    }
}