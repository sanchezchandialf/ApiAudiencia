using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Models;

public partial class AudienciasContext : DbContext
{
    public AudienciasContext()
    {
    }

    public AudienciasContext(DbContextOptions<AudienciasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Solicitud> Solicitudes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97DBEAD4C4");

            entity.ToTable("Usuario");

            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EsAdmin).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
            modelBuilder.Entity<Solicitud>()
                .HasOne(s => s.Empleado)
                .WithMany(e => e.Solicitudes)
                .HasForeignKey(s => s.Empleado.DNI);

            modelBuilder.Entity<Solicitud>()
                .HasOne(s => s.Proveedor)
                .WithMany(p => p.Solicitudes)
                .HasForeignKey(s => s.Proveedor.DNI);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
