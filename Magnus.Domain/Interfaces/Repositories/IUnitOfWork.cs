namespace Magnus.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUsuarioRepository Usuarios { get; }
        IOrganizadorRepository Organizadores { get; }
        IEventoRepository Eventos { get; }
        IEventoInvitadoRepository EventoInvitados { get; }
        IProveedorRepository Proveedores { get; }
        ICotizacionRepository Cotizaciones { get; }

        Task<int> CommitAsync();
    }
}
