using Magnus.Domain.Exceptions;

namespace Magnus.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private Usuario() { }

    public Usuario(string nombre, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nombre)) throw new DomainException("Nombre requerido.");
        if (string.IsNullOrWhiteSpace(Email)) throw new DomainException("Email requerido.");
    }
}