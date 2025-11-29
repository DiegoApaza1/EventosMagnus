using Magnus.Domain.Interfaces;
using Magnus.Domain.Entities;
using MediatR;

namespace Magnus.Application.Features.Proveedores.Queries.BuscarProveedores
{
    public class BuscarProveedoresQueryHandler : IRequestHandler<BuscarProveedoresQuery, IEnumerable<Proveedor>>
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

            var resultado = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtros.NombreContains))
                resultado = resultado.Where(p => p.Nombre != null && p.Nombre.Contains(filtros.NombreContains, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filtros.ServicioEquals))
                resultado = resultado.Where(p => p.Servicio != null && string.Equals(p.Servicio, filtros.ServicioEquals, StringComparison.OrdinalIgnoreCase));

            return resultado.ToList();
        }
    }
}