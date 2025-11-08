using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Magnus.Application.Interfaces;

namespace Magnus.Infrastructure.Persistence.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly MagnusDbContext _context;

        public ProveedorRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync() => await _context.Proveedores.ToListAsync();
        public async Task<Proveedor?> GetByIdAsync(int id) => await _context.Proveedores.FindAsync(id);
        public async Task AddAsync(Proveedor proveedor) => await _context.Proveedores.AddAsync(proveedor);
        public void Update(Proveedor proveedor) => _context.Proveedores.Update(proveedor);
        public void Delete(Proveedor proveedor) => _context.Proveedores.Remove(proveedor);
    }
}