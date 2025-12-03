using Magnus.Domain.Exceptions;

namespace Magnus.Domain.Entities;

public class Organizador
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string NombreEmpresa { get; private set; } = null!;
    public string? Descripcion { get; private set; }
    public string Telefono { get; private set; } = null!;
    public string? Direccion { get; private set; }
    public decimal PrecioPorEvento { get; private set; }
    public int AñosExperiencia { get; private set; }
    public string? Especialidad { get; private set; }
    public bool Verificado { get; private set; }
    public decimal Rating { get; private set; }
    public int CantidadReseñas { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Usuario? Usuario { get; private set; }

    private Organizador() { }

    public Organizador(
        Guid usuarioId,
        string nombreEmpresa,
        string telefono,
        decimal precioPorEvento,
        int añosExperiencia,
        string? descripcion = null,
        string? direccion = null,
        string? especialidad = null)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        NombreEmpresa = nombreEmpresa;
        Telefono = telefono;
        Descripcion = descripcion;
        Direccion = direccion;
        PrecioPorEvento = precioPorEvento;
        AñosExperiencia = añosExperiencia;
        Especialidad = especialidad;
        Verificado = false;
        Rating = 0;
        CantidadReseñas = 0;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    public void Actualizar(
        string nombreEmpresa,
        string telefono,
        decimal precioPorEvento,
        int añosExperiencia,
        string? descripcion = null,
        string? direccion = null,
        string? especialidad = null)
    {
        NombreEmpresa = nombreEmpresa;
        Telefono = telefono;
        PrecioPorEvento = precioPorEvento;
        AñosExperiencia = añosExperiencia;
        Descripcion = descripcion;
        Direccion = direccion;
        Especialidad = especialidad;
        Validate();
    }

    public void Verificar()
    {
        Verificado = true;
    }

    public void ActualizarRating(decimal nuevoRating, int nuevaCantidadReseñas)
    {
        if (nuevoRating < 0 || nuevoRating > 5)
            throw new DomainException("El rating debe estar entre 0 y 5.");
        
        Rating = nuevoRating;
        CantidadReseñas = nuevaCantidadReseñas;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(NombreEmpresa))
            throw new DomainException("El nombre de empresa es obligatorio.");
        
        if (string.IsNullOrWhiteSpace(Telefono))
            throw new DomainException("El teléfono es obligatorio.");
        
        if (PrecioPorEvento < 0)
            throw new DomainException("El precio debe ser mayor o igual a 0.");
        
        if (AñosExperiencia < 0)
            throw new DomainException("Los años de experiencia deben ser mayor o igual a 0.");
    }
}