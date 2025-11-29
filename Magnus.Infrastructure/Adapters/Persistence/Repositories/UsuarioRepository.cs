using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MagnusDbContext _context;

        public UsuarioRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync() 
            => await _context.Usuarios.ToListAsync();

        public async Task<Usuario?> GetByIdAsync(Guid id) 
            => await _context.Usuarios.FindAsync(id);

        public async Task<Usuario?> GetByEmailAsync(string email)
            => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(Usuario usuario) 
            => await _context.Usuarios.AddAsync(usuario);

        public void Update(Usuario usuario) 
            => _context.Usuarios.Update(usuario);

        public void Delete(Usuario usuario) 
            => _context.Usuarios.Remove(usuario);
    }
}