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

    public virtual DbSet<Audiencia> Audiencias { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Clasificacione> Clasificaciones { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HE1E4O7; Initial Catalog=Audiencias; Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audiencia>(entity =>
        {
            entity.HasKey(e => e.IdAudiencia).HasName("PK__Audienci__D24CE72D100830B4");

            entity.Property(e => e.Asunto).HasColumnType("text");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.DerivadoA).HasMaxLength(100);
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasColumnName("DNI");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreEmpresa).HasMaxLength(100);

            entity.HasOne(d => d.AtendidoPorNavigation).WithMany(p => p.Audiencia)
                .HasForeignKey(d => d.AtendidoPor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audiencia_Empleado");

            entity.HasOne(d => d.IdCargoNavigation).WithMany(p => p.Audiencia)
                .HasForeignKey(d => d.IdCargo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audiencia_Cargo");

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Audiencia)
                .HasForeignKey(d => d.IdClasificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audiencia_Clasificacion");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Audiencia)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audiencia_Estado");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargo__6C985625FA83D6F6");

            entity.ToTable("Cargo");

            entity.Property(e => e.NombreCargo).HasMaxLength(50);
        });

        modelBuilder.Entity<Clasificacione>(entity =>
        {
            entity.HasKey(e => e.Idclasificacion).HasName("PK__Clasific__28ABDECE9E13BC5F");

            entity.Property(e => e.Idclasificacion).HasColumnName("IDClasificacion");
            entity.Property(e => e.Clasificacion).HasMaxLength(50);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9EDEB47468");

            entity.ToTable("Empleado");

            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Idestado).HasName("PK__Estado__45504034F1EB29B8");

            entity.ToTable("Estado");

            entity.Property(e => e.Idestado).HasColumnName("IDEstado");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
