using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Commands.CrearOrganizador
{
    public record CrearOrganizadorCommand(string Nombre, string Telefono, Guid UsuarioId) 
        : IRequest<OrganizadorResponseDto>;
}
