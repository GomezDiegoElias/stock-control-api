using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Infrastructure.Persistence.MySql.Mappers
{
    public static class MysSqlRoleMapper
    {

        public static Role ToDomain(RoleEntity entity)
        {
            return new Role(
                entity.Name,
                entity.RolePermissions.Select(rp => rp.Permission.Name)
            );
        }

        public static RoleEntity toEntity(Role domain)
        {
            return new RoleEntity
            {
                Name = domain.Name,
                RolePermissions = domain.Permissions
                    .Select(p => new RolePermissionEntity { Permission = new PermissionEntity { Name = p } })
                    .ToList()
            };
        }

    }
}
