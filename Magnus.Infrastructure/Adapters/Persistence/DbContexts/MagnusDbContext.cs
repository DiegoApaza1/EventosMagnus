using Magnus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Adapters.Persistence.DbContexts
{
    public class MagnusDbContext : DbContext
    {
        public MagnusDbContext(DbContextOptions<MagnusDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<EventoInvitado> EventoInvitados { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional (ejemplo: relaciones y restricciones)
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Organizador)
                .WithMany()
                .HasForeignKey(e => e.OrganizadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cotizacion>()
                .HasOne(c => c.Evento)
                .WithMany()
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventoInvitado>()
                .HasOne(ei => ei.Evento)
                .WithMany()
                .HasForeignKey(ei => ei.EventoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventoInvitado>()
                .HasOne(ei => ei.Usuario)
                .WithMany()
                .HasForeignKey(ei => ei.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventoInvitado>()
                .HasIndex(ei => new { ei.EventoId, ei.UsuarioId })
                .IsUnique();
        }
    }
}