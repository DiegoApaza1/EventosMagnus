using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces.Repositories
{
    public interface IOrganizadorRepository
    {
        Task<IEnumerable<Organizador>> GetAllAsync();
        Task<Organizador?> GetByIdAsync(Guid id);

        Task<IEnumerable<Organizador>> SearchByNameAsync(string nombre);

        Task AddAsync(Organizador organizador);
        void Update(Organizador organizador);
        void Delete(Organizador organizador);
    }
}