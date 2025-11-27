using Magnus.Domain.Exceptions;
namespace Magnus.Domain.Entities;

public class Evento
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string? Descripcion { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public string Lugar { get; private set; } = null!;
    public int Capacidad { get; private set; }
    public Guid OrganizadorId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Evento() { }
    
    public Usuario? Organizador { get; private set; }

    public Evento(string titulo, DateTime inicio, DateTime fin, string lugar, int capacidad, Guid organizadorId, string? descripcion = null)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        FechaInicio = inicio;
        FechaFin = fin;
        Lugar = lugar;
        Capacidad = capacidad;
        OrganizadorId = organizadorId;
        Descripcion = descripcion;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    public void Update(string titulo, DateTime inicio, DateTime fin, string lugar, int capacidad, string? descripcion = null)
    {
        Titulo = titulo;
        FechaInicio = inicio;
        FechaFin = fin;
        Lugar = lugar;
        Capacidad = capacidad;
        Descripcion = descripcion;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Titulo)) throw new DomainException("El título es obligatorio.");
        if (FechaInicio >= FechaFin) throw new DomainException("La fecha de inicio debe ser anterior a la fecha de fin.");
        if (Capacidad < 0) throw new DomainException("La capacidad debe ser >= 0.");
    }
}