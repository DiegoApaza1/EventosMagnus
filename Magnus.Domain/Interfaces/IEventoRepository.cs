using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad Evento
    /// </summary>
    public interface IEventoRepository
    {
        // Consultas básicas
        Task<IEnumerable<Evento>> GetAllAsync();
        Task<Evento?> GetByIdAsync(Guid id);
        
        // Consultas específicas
        Task<IEnumerable<Evento>> GetByOrganizadorIdAsync(Guid organizadorId);
        
        // Comandos (no retornan Task porque EF trackea los cambios)
        Task AddAsync(Evento evento);
        void Update(Evento evento);
        void Delete(Evento evento);
    }
}