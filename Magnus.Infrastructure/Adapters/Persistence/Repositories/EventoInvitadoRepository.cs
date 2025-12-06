using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.Repositories
{
    public class EventoInvitadoRepository : IEventoInvitadoRepository
    {
        private readonly MagnusDbContext _context;

        public EventoInvitadoRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventoInvitado>> GetAllAsync()
            => await _context.EventoInvitados
                .Include(ei => ei.Evento)
                .Include(ei => ei.Usuario)
                .ToListAsync();

        public async Task<EventoInvitado?> GetByIdAsync(Guid id)
            => await _context.EventoInvitados
                .Include(ei => ei.Evento)
                .Include(ei => ei.Usuario)
                .FirstOrDefaultAsync(ei => ei.Id == id);

        public async Task<IEnumerable<EventoInvitado>> GetByEventoIdAsync(Guid eventoId)
            => await _context.EventoInvitados
                .Include(ei => ei.Usuario)
                .Where(ei => ei.EventoId == eventoId)
                .ToListAsync();

        public async Task<IEnumerable<EventoInvitado>> GetByUsuarioIdAsync(Guid usuarioId)
            => await _context.EventoInvitados
                .Include(ei => ei.Evento)
                .Where(ei => ei.UsuarioId == usuarioId)
                .ToListAsync();

        public async Task<EventoInvitado?> GetByEventoAndUsuarioAsync(Guid eventoId, Guid usuarioId)
            => await _context.EventoInvitados
                .FirstOrDefaultAsync(ei => ei.EventoId == eventoId && ei.UsuarioId == usuarioId);

        public async Task AddAsync(EventoInvitado eventoInvitado)
            => await _context.EventoInvitados.AddAsync(eventoInvitado);

        public void Update(EventoInvitado eventoInvitado)
            => _context.EventoInvitados.Update(eventoInvitado);

        public void Delete(EventoInvitado eventoInvitado)
            => _context.EventoInvitados.Remove(eventoInvitado);
    }
}
