using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositorio para la entidad Organizador.
    /// Maneja las operaciones CRUD relacionadas con los organizadores.
    /// </summary>
    public class OrganizadorRepository : IOrganizadorRepository
    {
        private readonly MagnusDbContext _context;

        public OrganizadorRepository(MagnusDbContext context)
        {
            _context = context;
        }

        // Consultas básicas
        public async Task<IEnumerable<Organizador>> GetAllAsync()
            => await _context.Organizadores.ToListAsync();

        public async Task<Organizador?> GetByIdAsync(Guid id)
            => await _context.Organizadores.FindAsync(id);

        // Consultas específicas (opcional)
        public async Task<IEnumerable<Organizador>> SearchByNameAsync(string nombre)
            => await _context.Organizadores
                .Where(o => o.Nombre.Contains(nombre))
                .ToListAsync();

        // Comandos
        public async Task AddAsync(Organizador organizador)
            => await _context.Organizadores.AddAsync(organizador);

        public void Update(Organizador organizador)
            => _context.Organizadores.Update(organizador);

        public void Delete(Organizador organizador)
            => _context.Organizadores.Remove(organizador);
    }
}