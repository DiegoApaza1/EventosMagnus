using System;

namespace Magnus.Domain.Entities
{
    public class Organizador
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public Guid UsuarioId { get; private set; }

        public Usuario Usuario { get; private set; } = null!;

        public Organizador(string nombre, string telefono, Guid usuarioId)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Telefono = telefono;
            UsuarioId = usuarioId;
        }

        private Organizador() { }
    }
}