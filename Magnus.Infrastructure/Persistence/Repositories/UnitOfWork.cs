using Magnus.Application.Interfaces;
using Magnus.Domain.Interfaces;
using Magnus.Infrastructure.Persistence.DbContexts;

namespace Magnus.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MagnusDbContext _context;

        public UnitOfWork(MagnusDbContext context)
        {
            _context = context;
            Usuarios = new UsuarioRepository(_context);
            Organizadores = new OrganizadorRepository(_context);
            Eventos = new EventoRepository(_context);
            Proveedores = new ProveedorRepository(_context);
            Cotizaciones = new CotizacionRepository(_context);
        }

        public IUsuarioRepository Usuarios { get; }
        public IOrganizadorRepository Organizadores { get; }
        public IEventoRepository Eventos { get; }
        public IProveedorRepository Proveedores { get; }
        public ICotizacionRepository Cotizaciones { get; }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        
        // Explicit implementation of IUnitOfWork.CompleteAsync
        async Task IUnitOfWork.CompleteAsync() => await _context.SaveChangesAsync();
    }
}