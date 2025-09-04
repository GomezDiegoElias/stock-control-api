using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Mappers
{
    public static class RoleMapper
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

        // Este mapper convierte un objeto de dominio Role a un DTO RoleApiResponse
        // Por defecto incluye permisos
        public static RoleApiResponse ToResponse(Role domain, bool includePermissions = true) 
        {
            return new RoleApiResponse(
                Name: domain.Name,
                // Evita exponer permisos si no se requiere
                Permissions: includePermissions ? domain.Permissions.ToList() : new List<string>() 
            );
        }

    }
}
