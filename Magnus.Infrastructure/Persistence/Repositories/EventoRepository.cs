using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
namespace Magnus.Infrastructure.Persistence.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly MagnusDbContext _context;

        public EventoRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> GetAllAsync() => await _context.Eventos.Include(e => e.Organizador).ToListAsync();
        public async Task<Evento?> GetByIdAsync(Guid id) => await _context.Eventos.Include(e => e.Organizador).FirstOrDefaultAsync(e => e.Id == id);
        public async Task AddAsync(Evento evento) => await _context.Eventos.AddAsync(evento);
        public void Update(Evento evento) => _context.Eventos.Update(evento);
        public void Delete(Evento evento) => _context.Eventos.Remove(evento);

        public async Task<IEnumerable<Evento>> GetByOrganizadorIdAsync(Guid organizadorId)
            => await _context.Eventos.Where(e => e.OrganizadorId == organizadorId).ToListAsync();
    }
}