using System;
using Microsoft.EntityFrameworkCore;
using GestionContactosApi.Models;

namespace ContApi.Models
{
    public partial class DbA358b2Pam3Context : DbContext
    {
        public DbA358b2Pam3Context()
        {
        }

        public DbA358b2Pam3Context(DbContextOptions<DbA358b2Pam3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Contacto> Contacto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=SQL8020.site4now.net;Database=db_a358b2_pam3;User Id=db_a358b2_pam3_admin;Password=tudai123;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.HasKey(e => e.ContactoId);

                entity.ToTable("Contacto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Apellido)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                             .HasMaxLength(150)
                             .IsUnicode(false);

                entity.Property(e => e.Activo)
                        .HasMaxLength(150)
                        .IsUnicode(false);

            });


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.NombreApellido)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
