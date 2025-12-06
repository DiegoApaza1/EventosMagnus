using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.Repositories
{
    public class OrganizadorRepository : IOrganizadorRepository
    {
        private readonly MagnusDbContext _context;

        public OrganizadorRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organizador>> GetAllAsync()
            => await _context.Organizadores.ToListAsync();

        public async Task<Organizador?> GetByIdAsync(Guid id)
            => await _context.Organizadores.FindAsync(id);

        public async Task<Organizador?> GetByUsuarioIdAsync(Guid usuarioId)
            => await _context.Organizadores.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId);

        public async Task<IEnumerable<Organizador>> SearchByNameAsync(string nombre)
            => await _context.Organizadores
                .Where(o => o.NombreEmpresa.Contains(nombre))
                .ToListAsync();

        public async Task AddAsync(Organizador organizador)
            => await _context.Organizadores.AddAsync(organizador);

        public void Update(Organizador organizador)
            => _context.Organizadores.Update(organizador);

        public void Delete(Organizador organizador)
            => _context.Organizadores.Remove(organizador);
    }
}