using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;

namespace org.pos.software.Infrastructure.Persistence.SqlServer
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }

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
                .Property(u => u.Status)
                .HasConversion<string>(); // Almacenar Status como string

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .IsRequired();

            modelBuilder.Entity<RolePermissionEntity>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .IsRequired();

        }

    }
}
