using Magnus.Application.Interfaces;
using Magnus.Domain.Entities;

namespace Magnus.Application.Features.Proveedores.Queries.BuscarProveedores
{
    public class BuscarProveedoresQueryHandler
    {
        private readonly IUnitOfWork _uow;

        public BuscarProveedoresQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Proveedor>> Handle(BuscarProveedoresQuery query, CancellationToken ct = default)
        {
            var filtros = query.Filtros;
            var todos = await _uow.Proveedores.GetAllAsync();

            // Aplicar filtro simple en memoria (si la tabla es muy grande, refactoriza para aplicar filtros en repositorio)
            var resultado = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtros.NombreContains))
                resultado = resultado.Where(p => p.Nombre != null && p.Nombre.Contains(filtros.NombreContains, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filtros.ServicioEquals))
                resultado = resultado.Where(p => p.Servicio != null && string.Equals(p.Servicio, filtros.ServicioEquals, StringComparison.OrdinalIgnoreCase));

            return resultado.ToList();
        }
    }
}