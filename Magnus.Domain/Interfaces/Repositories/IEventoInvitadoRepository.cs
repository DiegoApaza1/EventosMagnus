using Magnus.Domain.Entities;

namespace Magnus.Domain.Interfaces.Repositories
{
    public interface IEventoInvitadoRepository
    {
        Task<IEnumerable<EventoInvitado>> GetAllAsync();
        Task<EventoInvitado?> GetByIdAsync(Guid id);
        Task<IEnumerable<EventoInvitado>> GetByEventoIdAsync(Guid eventoId);
        Task<IEnumerable<EventoInvitado>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<EventoInvitado?> GetByEventoAndUsuarioAsync(Guid eventoId, Guid usuarioId);
        Task AddAsync(EventoInvitado eventoInvitado);
        void Update(EventoInvitado eventoInvitado);
        void Delete(EventoInvitado eventoInvitado);
    }
}
