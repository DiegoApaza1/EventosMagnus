using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ListarOrganizadores
{
    public record ListarOrganizadoresQuery : IRequest<IEnumerable<OrganizadorResponseDto>>;
}
