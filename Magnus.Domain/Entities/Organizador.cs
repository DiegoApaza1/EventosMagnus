using System;

namespace Magnus.Domain.Entities
{
    public class Organizador
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Telefono { get; private set; }
        public Guid UsuarioId { get; private set; }

        public Usuario Usuario { get; private set; }

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