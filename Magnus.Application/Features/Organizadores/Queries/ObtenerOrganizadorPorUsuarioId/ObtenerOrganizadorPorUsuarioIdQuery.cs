using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorUsuarioId
{
    public record ObtenerOrganizadorPorUsuarioIdQuery(Guid UsuarioId) : IRequest<OrganizadorResponseDto?>;
}
