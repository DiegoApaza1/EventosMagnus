namespace Magnus.Domain.Entities;

public class Organizador
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string? Telefono { get; private set; }

    private Organizador() { }

    public Organizador(string nombre, string? telefono = null)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Telefono = telefono;
    }
}