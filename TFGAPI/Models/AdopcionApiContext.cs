using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TFGAPI.Models;

public partial class AdopcionApiContext : DbContext
{
    public AdopcionApiContext()
    {
    }

    public AdopcionApiContext(DbContextOptions<AdopcionApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Mensaje> Mensajes { get; set; }

    public virtual DbSet<Publicacion> Publicaciones { get; set; }

    public virtual DbSet<PublicacionesFavoritas> PublicacionesFavoritas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:server.windows.net,1433;Initial Catalog=AdopcionAPI;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.UsuarioIniciadorId).HasColumnName("usuarioIniciadorID");
            entity.Property(e => e.UsuarioPublicacionId).HasColumnName("usuarioPublicacionID");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.Property(e => e.MensajeId).HasColumnName("MensajeID");
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.Contenido).HasColumnName("contenido");
            entity.Property(e => e.FechaEnvio)
                .HasColumnType("date")
                .HasColumnName("fechaEnvio");
            entity.Property(e => e.NombreEmisor)
                .HasMaxLength(50)
                .HasColumnName("nombreEmisor");
            entity.Property(e => e.UsuarioEmisorId).HasColumnName("UsuarioEmisorID");
            entity.Property(e => e.UsuarioReceptorId).HasColumnName("UsuarioReceptorID");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.HasKey(e => e.PublicacionId);

            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .HasColumnName("ciudad");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.EdadAnimal).HasColumnName("edadAnimal");
            entity.Property(e => e.EspecieAnimal)
                .HasMaxLength(50)
                .HasColumnName("especieAnimal");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("date")
                .HasColumnName("fechaPublicacion");
            entity.Property(e => e.FotoAnimal).HasColumnName("fotoAnimal");
            entity.Property(e => e.NombreAnimal)
                .HasMaxLength(50)
                .HasColumnName("nombreAnimal");
            entity.Property(e => e.NumInteresados).HasColumnName("numInteresados");
            entity.Property(e => e.SexoAnimal)
                .HasMaxLength(50)
                .HasColumnName("sexoAnimal");
            entity.Property(e => e.TamanyoAnimal)
                .HasMaxLength(50)
                .HasColumnName("tamanyoAnimal");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<PublicacionesFavoritas>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .HasColumnName("ciudad");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EsProtectora).HasMaxLength(1);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .HasColumnName("telefono");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });
        modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
