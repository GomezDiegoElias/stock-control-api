using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Utils.Seeder
{
    public static class MySqlDbSedder
    {



        public static async Task SeedRolesAndPermissions(MySqlDbContext context)
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

        public static async Task SeedUserWhitDiferentRoles(MySqlDbContext context)
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
                    Email = "gomezdiegoelias1@gmail.com",
                    FirstName = "Diego",
                    Role = roleAdmin,
                    RoleId = roleAdmin.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordDiego,
                    Salt = saltDiego
                };

                // ---- USUARIO 2 ----
                string passwordLeo = "123456";
                string saltLeo = PasswordUtils.GenerateRandomSalt();
                string hashedPasswordLeo = PasswordUtils.HashPasswordWithSalt(passwordLeo, saltLeo);

                var Leo = new UserEntity
                {
                    Id = User.GenerateId(),
                    Dni = 12345678,
                    Email = "example123@gmail.com",
                    FirstName = "Leo",
                    Role = rolePresupuestista,
                    RoleId = rolePresupuestista.Id,
                    Status = Status.ACTIVE,
                    Hash = hashedPasswordLeo,
                    Salt = saltLeo
                };

                context.Users.AddRange(Diego, Leo);
                await context.SaveChangesAsync();

            }

        }

        // ---- SEEDER PARA CREACION DE PROCEDIMIENTOS ALMACENADOS ----

        ///////////////////////////////////////////////////////////////

    }
}
