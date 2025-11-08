using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para la entidad Usuario
    /// </summary>
    public interface IUsuarioRepository
    {
        // Consultas básicas
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        
        // Consultas específicas para autenticación
        Task<Usuario?> GetByEmailAsync(string email);
        
        // Comandos
        Task AddAsync(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
    }
}