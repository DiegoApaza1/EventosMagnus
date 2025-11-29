using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Adapters.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.Repositories
{
    public class CotizacionRepository : ICotizacionRepository
    {
        private readonly MagnusDbContext _context;

        public CotizacionRepository(MagnusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cotizacion>> GetAllAsync() => await _context.Cotizaciones.ToListAsync();
        public async Task<Cotizacion?> GetByIdAsync(Guid id) => await _context.Cotizaciones.FindAsync(id);
        
        public async Task AddAsync(Cotizacion cotizacion) => await _context.Cotizaciones.AddAsync(cotizacion);
        public void Update(Cotizacion cotizacion) => _context.Cotizaciones.Update(cotizacion);
        public void Delete(Cotizacion cotizacion) => _context.Cotizaciones.Remove(cotizacion);
        public async Task<IEnumerable<Cotizacion>> GetByEventoIdAsync(Guid eventoId)
            => await _context.Cotizaciones.Where(c => c.EventoId == eventoId).ToListAsync();
    }
}