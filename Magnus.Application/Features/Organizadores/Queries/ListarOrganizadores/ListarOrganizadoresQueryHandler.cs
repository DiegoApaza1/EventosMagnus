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
                NombreEmpresa = o.NombreEmpresa,
                Descripcion = o.Descripcion,
                Telefono = o.Telefono,
                Direccion = o.Direccion,
                PrecioPorEvento = o.PrecioPorEvento,
                AñosExperiencia = o.AñosExperiencia,
                Especialidad = o.Especialidad,
                Verificado = o.Verificado,
                Rating = o.Rating,
                UsuarioId = o.UsuarioId
            });
        }
    }
}
