using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces.Repositories
{
    public interface IEventoRepository
    {
        Task<IEnumerable<Evento>> GetAllAsync();
        Task<Evento?> GetByIdAsync(Guid id);
        
        Task<IEnumerable<Evento>> GetByOrganizadorIdAsync(Guid organizadorId);
        
        Task AddAsync(Evento evento);
        void Update(Evento evento);
        void Delete(Evento evento);
    }
}