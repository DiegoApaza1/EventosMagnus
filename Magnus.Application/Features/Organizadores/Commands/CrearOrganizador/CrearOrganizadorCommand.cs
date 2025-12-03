using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Commands.CrearOrganizador
{
    public record CrearOrganizadorCommand(
        string NombreEmpresa,
        string Telefono,
        decimal PrecioPorEvento,
        int AñosExperiencia,
        Guid UsuarioId,
        string? Descripcion = null,
        string? Direccion = null,
        string? Especialidad = null) 
        : IRequest<OrganizadorResponseDto>;
}
