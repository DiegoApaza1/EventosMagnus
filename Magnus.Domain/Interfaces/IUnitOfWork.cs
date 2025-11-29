namespace Magnus.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUsuarioRepository Usuarios { get; }
        IOrganizadorRepository Organizadores { get; }
        IEventoRepository Eventos { get; }
        IProveedorRepository Proveedores { get; }
        ICotizacionRepository Cotizaciones { get; }

        Task<int> CommitAsync();
    }
}
