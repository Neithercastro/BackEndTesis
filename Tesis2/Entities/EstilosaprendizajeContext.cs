using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tesis2.Entities;

public partial class EstilosaprendizajeContext : DbContext
{
    public EstilosaprendizajeContext()
    {
    }

    public EstilosaprendizajeContext(DbContextOptions<EstilosaprendizajeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contenidodetalle> Contenidodetalles { get; set; }

    public virtual DbSet<Estilosaprendizaje> Estilosaprendizajes { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Materiadetalle> Materiadetalles { get; set; }

    public virtual DbSet<Semestre> Semestres { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Verificacion> Verificacions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contenidodetalle>(entity =>
        {
            entity.HasKey(e => e.IdContenido).HasName("PRIMARY");

            entity.ToTable("contenidodetalle");

            entity.HasIndex(e => e.IdActividad, "FK_IdActividad_idx");

            entity.HasIndex(e => e.IdEstiloAprendizaje, "IdEstiloAprendizaje_idx");

            entity.HasIndex(e => e.IdMateria, "materia_idx");

            entity.Property(e => e.IdContenido).HasColumnName("idContenido");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.IdEstiloAprendizaje).HasColumnName("idEstiloAprendizaje");
            entity.Property(e => e.IdMateria).HasColumnName("idMateria");
            entity.Property(e => e.IdActividad).HasColumnName("idactividad");
            entity.Property(e => e.MaterialApoyo1).HasMaxLength(45);
            entity.Property(e => e.MaterialApoyo2).HasMaxLength(45);
            
            entity.HasOne(d => d.IdMateriaDetalleNavigation).WithMany(p => p.Contenidodetalles)
            .HasForeignKey(d => d.IdActividad);
                        //.HasConstraintName("FK_IdActividad");

            entity.HasOne(d => d.IdEstiloAprendizajeNavigation).WithMany(p => p.Contenidodetalles)
                .HasForeignKey(d => d.IdEstiloAprendizaje);
            //.HasConstraintName("IdEstiloAprendizaje");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Contenidodetalles)
                .HasForeignKey(d => d.IdMateria);
               // .HasConstraintName("materia");


        });

        modelBuilder.Entity<Estilosaprendizaje>(entity =>
        {
            entity.HasKey(e => e.IdestilosAprendizaje).HasName("PRIMARY");

            entity.ToTable("estilosaprendizaje");

            entity.Property(e => e.IdestilosAprendizaje).HasColumnName("idestilosAprendizaje");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.IdMaterias).HasName("PRIMARY");

            entity.ToTable("materias");

            entity.HasIndex(e => e.Semestre, "FK_Semestre_idx");

            entity.Property(e => e.IdMaterias).HasColumnName("idMaterias");
            entity.Property(e => e.DireccionImagen).HasMaxLength(45);
            entity.Property(e => e.Nombre).HasMaxLength(45);

            entity.HasOne(d => d.SemestreNavigation).WithMany(p => p.Materia)
                .HasForeignKey(d => d.Semestre)
                .HasConstraintName("FK_Semestre");
        });

        modelBuilder.Entity<Materiadetalle>(entity =>
        {
            entity.HasKey(e => new { e.IdMateriaDetalle }).HasName("PRIMARY");

            entity.ToTable("materiadetalle");

            entity.HasIndex(e => e.IdMateria, "idMateria_idx");

            entity.Property(e => e.IdMateriaDetalle)
                .ValueGeneratedOnAdd()
                .HasColumnName("idMateriaDetalle");
            entity.Property(e => e.NombreActividad).HasMaxLength(200);

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Materiadetalles)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("FK_idMateria");
        });

        modelBuilder.Entity<Semestre>(entity =>
        {
            entity.HasKey(e => e.IdSemestres).HasName("PRIMARY");

            entity.ToTable("semestres");

            entity.Property(e => e.IdSemestres).HasColumnName("idSemestres");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => new { e.Idusuarios }).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Semestres, "FK_NumSemestre_idx");

            entity.HasIndex(e => e.Idestilos, "IdEstilosAprendizaje_idx");

            entity.Property(e => e.Idusuarios)
                .ValueGeneratedOnAdd()
                .HasColumnName("idusuarios");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(45)
                .HasColumnName("Usuario");
            entity.Property(e => e.Contraseña).HasMaxLength(45);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(45)
                .HasColumnName("Correo Electronico");
            entity.Property(e => e.Idestilos).HasColumnName("idestilos");
            entity.Property(e => e.Nombre).HasMaxLength(45);

            entity.HasOne(d => d.IdestilosNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Idestilos)
                .HasConstraintName("IdEstilosAprendizaje");

            entity.HasOne(d => d.SemestresNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Semestres)
                .HasConstraintName("FK_NumSemestre");
        });

        modelBuilder.Entity<Verificacion>(entity =>
        {
            entity.HasKey(e => e.Idverificacion).HasName("PRIMARY");

            entity.ToTable("verificacion");

            entity.Property(e => e.Idverificacion).HasColumnName("idverificacion");
            entity.Property(e => e.Permiso).HasMaxLength(45);
            entity.Property(e => e.Token).HasMaxLength(200);
            entity.Property(e => e.Usuario).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
