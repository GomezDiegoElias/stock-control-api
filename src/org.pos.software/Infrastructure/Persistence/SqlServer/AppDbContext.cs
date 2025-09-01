using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;
using System.Data;

namespace org.pos.software.Infrastructure.Persistence.SqlServer
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }

        // Metodo para la paginacion de usuarios
        public async Task<PaginatedResponse<User>> getUserPagination(int pageIndex, int pageSize)
        {
            var users = new List<User>();
            var totalCount = 0;

            using var connection = new SqlConnection(Database.GetConnectionString());
            await connection.OpenAsync();

            using var command = new SqlCommand("getUserPagination", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageIndex", pageIndex);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(User.Builder()
                    .Id(reader["id"].ToString() ?? string.Empty)
                    .Dni(Convert.ToInt64(reader["dni"]))
                    .Email(reader["email"].ToString() ?? string.Empty)
                    .FirstName(reader["first_name"].ToString() ?? string.Empty)
                    .LastName(reader["last_name"].ToString() ?? string.Empty)
                    .Role(new Role(reader["role"].ToString() ?? string.Empty, Enumerable.Empty<string>()))
                    .Status(Enum.Parse<Status>(reader["status"].ToString() ?? string.Empty))
                    .Build()
                );

                if (totalCount == 0)
                {
                    totalCount = Convert.ToInt32(reader["TotalFilas"]);
                }
            }

            await connection.CloseAsync();

            return new PaginatedResponse<User>
            {
                Items = users,
                TotalCount = totalCount
            };
        }


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
