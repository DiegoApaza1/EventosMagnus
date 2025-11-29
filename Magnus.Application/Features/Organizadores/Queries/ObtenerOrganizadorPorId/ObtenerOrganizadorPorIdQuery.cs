using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorId
{
    public record ObtenerOrganizadorPorIdQuery(Guid Id) : IRequest<OrganizadorResponseDto?>;
}
