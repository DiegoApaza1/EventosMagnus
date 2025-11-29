using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using MediatR;

namespace Magnus.Application.Features.Proveedores.Queries.BuscarProveedores
{
    public class BuscarProveedoresQuery : IRequest<IEnumerable<Proveedor>>
    {
        public ProveedorBusquedaDto Filtros { get; }

        public BuscarProveedoresQuery(ProveedorBusquedaDto filtros)
        {
            Filtros = filtros;
        }
    }
}