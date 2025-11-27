using Magnus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magnus.Infrastructure.Persistence.DbContexts
{
    public class MagnusDbContext : DbContext
    {
        public MagnusDbContext(DbContextOptions<MagnusDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}