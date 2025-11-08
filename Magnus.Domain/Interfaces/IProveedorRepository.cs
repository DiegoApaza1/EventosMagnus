using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad Proveedor
    /// </summary>
    public interface IProveedorRepository
    {
        // Consultas básicas
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(Guid id);
        
        // Consultas específicas
        Task<IEnumerable<Proveedor>> SearchByNameOrServiceAsync(string searchTerm);
        
        // Comandos
        Task AddAsync(Proveedor proveedor);
        void Update(Proveedor proveedor);
        void Delete(Proveedor proveedor);
    }
}