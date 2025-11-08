using Magnus.Application.DTOs;

namespace Magnus.Application.Features.Proveedores.Queries.BuscarProveedores
{
    public class BuscarProveedoresQuery
    {
        public ProveedorBusquedaDto Filtros { get; }

        public BuscarProveedoresQuery(ProveedorBusquedaDto filtros)
        {
            Filtros = filtros;
        }
    }
}