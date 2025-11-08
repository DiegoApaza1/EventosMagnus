using Magnus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magnus.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        Task AddAsync(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
    }
}