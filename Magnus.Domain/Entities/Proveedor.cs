namespace Magnus.Domain.Entities;

public class Proveedor
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string? Servicio { get; private set; }

    private Proveedor() { }

    public Proveedor(string nombre, string? servicio = null)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Servicio = servicio;
    }
}