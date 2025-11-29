using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ListarOrganizadores
{
    public class ListarOrganizadoresQueryHandler : IRequestHandler<ListarOrganizadoresQuery, IEnumerable<OrganizadorResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public ListarOrganizadoresQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<OrganizadorResponseDto>> Handle(ListarOrganizadoresQuery request, CancellationToken cancellationToken)
        {
            var organizadores = await _uow.Organizadores.GetAllAsync();

            return organizadores.Select(o => new OrganizadorResponseDto
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Telefono = o.Telefono,
                UsuarioId = o.UsuarioId
            });
        }
    }
}
