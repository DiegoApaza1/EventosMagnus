using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    public interface ICotizacionRepository
    {
        Task<IEnumerable<Cotizacion>> GetAllAsync();
        Task<Cotizacion?> GetByIdAsync(Guid id);
        Task AddAsync(Cotizacion cotizacion);
        void Update(Cotizacion cotizacion);
        void Delete(Cotizacion cotizacion);
        Task<IEnumerable<Cotizacion>> GetByEventoIdAsync(Guid eventoId);
    }
}