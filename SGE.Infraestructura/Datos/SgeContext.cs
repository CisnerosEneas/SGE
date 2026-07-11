using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public class SgeContext : DbContext
{
    public SgeContext(DbContextOptions<SgeContext> options) : base(options) { }

    public DbSet<Expediente> Expedientes => Set<Expediente>();
    public DbSet<Tramite> Tramites => Set<Tramite>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nombre).IsRequired();
            builder.Property(u => u.CorreoElectronico).IsRequired();
            builder.Property(u => u.ContrasenaHash).IsRequired();
            builder.Property(u => u.EsAdministrador).IsRequired();

            builder.Property<List<Permiso>>("_permisos")
                .HasField("_permisos")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    permisos => string.Join(',', permisos.Select(p => (int)p)),
                    texto => string.IsNullOrWhiteSpace(texto)
                        ? new List<Permiso>()
                        : texto.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => (Permiso)int.Parse(x)).ToList())
                .HasColumnName("Permisos");
        });

        modelBuilder.Entity<Expediente>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.ComplexProperty(e => e.Caratula, caratulaBuilder =>
            {
                caratulaBuilder.Property(c => c.Texto).HasColumnName("Caratula").IsRequired();
            });
            builder.Property(e => e.Estado).IsRequired();
            builder.Property(e => e.FechaCreacion).IsRequired();
            builder.Property(e => e.FechaUltimaModificacion).IsRequired();
            builder.Property(e => e.UsuarioUltimoCambio).IsRequired();
        });

        modelBuilder.Entity<Tramite>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.ExpedienteId).IsRequired();
            builder.Property(t => t.Etiqueta).IsRequired();
            builder.ComplexProperty(t => t.Contenido, contenidoBuilder =>
            {
                contenidoBuilder.Property(c => c.Texto).HasColumnName("Contenido").IsRequired();
            });
            builder.Property(t => t.FechaCreacion).IsRequired();
            builder.Property(t => t.FechaUltimaModificacion).IsRequired();
            builder.Property(t => t.UsuarioUltimoCambio).IsRequired();
        });
    }
}