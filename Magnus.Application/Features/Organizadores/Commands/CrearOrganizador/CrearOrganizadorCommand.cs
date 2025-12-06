using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Commands.CrearOrganizador
{
    public record CrearOrganizadorCommand(
        string NombreEmpresa,
        string Descripcion,
        string Telefono,
        string Direccion,
        decimal PrecioPorEvento,
        int AñosExperiencia,
        string Especialidad,
        Guid UsuarioId) 
        : IRequest<OrganizadorResponseDto>;
}
