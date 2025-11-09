using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad Organizador
    /// </summary>
    public interface IOrganizadorRepository
    {
        // Consultas básicas
        Task<IEnumerable<Organizador>> GetAllAsync();
        Task<Organizador?> GetByIdAsync(Guid id);

        // Consultas específicas
        Task<IEnumerable<Organizador>> SearchByNameAsync(string nombre);

        // Comandos
        Task AddAsync(Organizador organizador);
        void Update(Organizador organizador);
        void Delete(Organizador organizador);
    }
}