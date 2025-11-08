using Magnus.Domain.Interfaces;
using System.Threading.Tasks;

namespace Magnus.Application.Interfaces
{
    /// <summary>
    /// Unit of Work expone los repositorios y el Commit.
    /// Implementación real está en Magnus.Infrastructure (UnitOfWork).
    /// </summary>
    public interface IUnitOfWork
    {
        IUsuarioRepository Usuarios { get; }
        IEventoRepository Eventos { get; }
        IProveedorRepository Proveedores { get; }
        ICotizacionRepository Cotizaciones { get; }

        Task<int> CommitAsync();
    }
}