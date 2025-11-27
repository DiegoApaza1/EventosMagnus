using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(Guid id);
        
        Task<IEnumerable<Proveedor>> SearchByNameOrServiceAsync(string searchTerm);
        
        Task AddAsync(Proveedor proveedor);
        void Update(Proveedor proveedor);
        void Delete(Proveedor proveedor);
    }
}