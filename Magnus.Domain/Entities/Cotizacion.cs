namespace Magnus.Domain.Entities;

public class Cotizacion
{
    public Guid Id { get; private set; }
    public Guid EventoId { get; private set; }
    public Guid ProveedorId { get; private set; }
    public decimal Monto { get; private set; }
    public DateTime Fecha { get; private set; }
    
    public Evento? Evento { get; private set; }

    private Cotizacion() {}

    public Cotizacion(Guid eventoId, Guid proveedorId, decimal monto)
    {
        Id = Guid.NewGuid();
        EventoId = eventoId;
        ProveedorId = proveedorId;
        Monto = monto;
        Fecha = DateTime.UtcNow;
    }
}