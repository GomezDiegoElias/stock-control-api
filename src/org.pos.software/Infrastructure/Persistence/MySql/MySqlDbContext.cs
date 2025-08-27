using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Infrastructure.Persistence.MySql
{
    public class MySqlDbContext : DbContext
    {

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        // Configuracion del modelo de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email) // Indice unico en el campo Email
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Dni) // Indice unico en el campo Dni
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Role)
                .HasConversion<string>(); // Almacenar Role como string

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Status)
                .HasConversion<string>(); // Almacenar Status como string

        }

    }
}
