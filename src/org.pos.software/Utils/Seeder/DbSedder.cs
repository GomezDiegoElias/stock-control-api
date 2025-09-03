using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;

namespace org.pos.software.Utils.Seeder
{
    public static class DbSedder
    {

        public static async Task SeedRolesAndPermissions(AppDbContext context)
        {

            if (!context.Roles.Any())
            {

                // Crear permisos
                var permRead = new PermissionEntity { Name = "READ" };
                var permCreate = new PermissionEntity { Name = "CREATE" };
                context.Permissions.Add(permRead);

                // Crear roles
                var presupuestista = new RoleEntity
                {
                    Name = "PRESUPUESTISTA",
                    RolePermissions = new List<RolePermissionEntity>
                    {
                        new RolePermissionEntity { Permission = permRead }
                    }
                };

                var admin = new RoleEntity
                {
                    Name = "ADMIN",
                    RolePermissions = new List<RolePermissionEntity>
                    {
                        new RolePermissionEntity { Permission = permRead },
                        new RolePermissionEntity { Permission = permCreate }
                    }
                };

                context.Roles.AddRange(presupuestista, admin);

                await context.SaveChangesAsync();
            }

        }

        public static async Task SeedUserWhitDiferentRoles(AppDbContext context)
        {

            if (!context.Users.Any())
            {

                var rolePresupuestista = await context.Roles.FirstOrDefaultAsync(x => x.Name == "PRESUPUESTISTA");
                if (rolePresupuestista == null)
                    throw new ApplicationException("Role PRESUPUESTISTA does not exist");

                var roleAdmin = await context.Roles.FirstOrDefaultAsync(x => x.Name == "ADMIN");
                if (roleAdmin == null)
                    throw new ApplicationException("Role ADMIN does not exist");

                // ---- USUARIO 1 ----
                string passwordDiego = "123456";
                string saltDiego = PasswordUtils.GenerateRandomSalt();
                string hashedPasswordDiego = PasswordUtils.HashPasswordWithSalt(passwordDiego, saltDiego);

                var Diego = new UserEntity
                {
                    Id = User.GenerateId(),
                    Dni = 46924236,
                    Email = "micorreodiego@gmail.com",
                    FirstName = "Diego",
                    LastName = "Gomez",
                    Role = roleAdmin,
                    RoleId = roleAdmin.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordDiego,
                    Salt = saltDiego
                };

                // ---- USUARIO 2 ----
                string passwordHugo = "123456";
                string saltHugo = PasswordUtils.GenerateRandomSalt();
                string hashedPasswordHugo = PasswordUtils.HashPasswordWithSalt(passwordHugo, saltHugo);

                var Hugo = new UserEntity
                {
                    Id = User.GenerateId(),
                    Dni = 12345678,
                    Email = "micorreohugo@gmail.com",
                    FirstName = "Hugo",
                    LastName = "Brocal",
                    Role = rolePresupuestista,
                    RoleId = rolePresupuestista.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordHugo,
                    Salt = saltHugo
                };

                // ---- USUARIO 3 ----
                string passwordJoel = "123456";
                string saltJoel = PasswordUtils.GenerateRandomSalt();
                string hashedPasswordJoel = PasswordUtils.HashPasswordWithSalt(passwordJoel, saltJoel);

                var Joel = new UserEntity
                {
                    Id = User.GenerateId(),
                    Dni = 87654321,
                    Email = "micorreojoel@gmail.com",
                    FirstName = "Joel",
                    LastName = "Trolson",
                    Role = rolePresupuestista,
                    RoleId = rolePresupuestista.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordJoel,
                    Salt = saltJoel
                };

                // ---- USUARIO 4 ----
                string passwordMatias = "123456";
                string saltMatias = PasswordUtils.GenerateRandomSalt();
                string hashedPasswordMatias = PasswordUtils.HashPasswordWithSalt(passwordMatias, saltMatias);

                var Matias = new UserEntity
                {
                    Id = User.GenerateId(),
                    Dni = 13548654,
                    Email = "micorreomatias@gmail.com",
                    FirstName = "Matias",
                    LastName = "Geymonat",
                    Role = rolePresupuestista,
                    RoleId = rolePresupuestista.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordMatias,
                    Salt = saltMatias
                };

                context.Users.AddRange(Diego, Hugo, Joel, Matias);
                await context.SaveChangesAsync();

            }

        }

        // ---- SEEDER PARA CREACION DE PROCEDIMIENTOS ALMACENADOS ----
        public static async Task SeedStoredProceduresPaginationUser(AppDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync(@"
            IF OBJECT_ID('getUserPagination', 'P') IS NOT NULL
                DROP PROCEDURE getUserPagination;
            ");

            await context.Database.ExecuteSqlRawAsync(@"
                CREATE PROCEDURE getUserPagination 
                    @PageIndex INT = 1,
                    @PageSize INT = 10
                AS
                BEGIN
                    DECLARE @Offset INT = (@PageSize * (@PageIndex - 1));

                    SELECT
                        u.id,
                        u.dni,
                        u.email,
                        u.first_name,
                        u.last_name,
                        r.name as role,
                        u.Status,
                        ROW_NUMBER() OVER(ORDER BY u.id ASC) AS Fila,
                        COUNT(*) OVER() AS TotalFilas
                    FROM tbl_user u
                    JOIN tbl_role r on u.role_id = r.id
                    ORDER BY u.id ASC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY
                END
            ");
        }

        public static async Task SeedStoredProceduresPaginationClient(AppDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync(@"
            IF OBJECT_ID('getClientPagination', 'P') IS NOT NULL
                DROP PROCEDURE getClientPagination;
            ");

            await context.Database.ExecuteSqlRawAsync(@"
                CREATE PROCEDURE getClientPagination 
                    @PageIndex INT = 1,
                    @PageSize INT = 10
                AS
                BEGIN
                    DECLARE @Offset INT = (@PageSize * (@PageIndex - 1));

                    SELECT
                        c.id,
                        c.dni,
                        c.first_name,
		                c.[address],
                        ROW_NUMBER() OVER(ORDER BY c.dni ASC) AS Fila,
                        COUNT(*) OVER() AS TotalFilas
                    FROM tbl_client c
	                WHERE c.is_deleted = 0
                    ORDER BY c.first_name ASC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY
                END
            ");
        }

        ///////////////////////////////////////////////////////////////

    }
}
