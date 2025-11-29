using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly MagnusDbContext _context;

        public ProveedorRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync() 
            => await _context.Proveedores.ToListAsync();

        public async Task<Proveedor?> GetByIdAsync(Guid id) 
            => await _context.Proveedores.FindAsync(id);

        public async Task<IEnumerable<Proveedor>> SearchByNameOrServiceAsync(string searchTerm)
            => await _context.Proveedores
                .Where(p => p.Nombre.Contains(searchTerm) || (p.Servicio != null && p.Servicio.Contains(searchTerm)))
                .ToListAsync();

        public async Task AddAsync(Proveedor proveedor) 
            => await _context.Proveedores.AddAsync(proveedor);

        public void Update(Proveedor proveedor) 
            => _context.Proveedores.Update(proveedor);

        public void Delete(Proveedor proveedor) 
            => _context.Proveedores.Remove(proveedor);
    }
}