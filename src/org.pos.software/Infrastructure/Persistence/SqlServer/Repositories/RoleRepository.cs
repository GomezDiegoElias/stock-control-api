using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> FindByName(string name)
        {
            var entity = await _context.Roles
                            .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                            .FirstOrDefaultAsync(r => r.Name == name);
            //if (entity == null) return null;
            return RoleMapper.ToDomain(entity);
        }

        public async Task<Role> Save(Role role)
        {
            var entity = RoleMapper.toEntity(role);
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return RoleMapper.ToDomain(entity);
        }

        public async Task<Role> UpdatePermissions(string roleName, IEnumerable<string> addPermissions, IEnumerable<string> removePermissions)
        {
            
            var roleEntity = _context.Roles
                                .Include(r => r.RolePermissions)
                                .ThenInclude(rp => rp.Permission)
                                .FirstOrDefault(r => r.Name == roleName);

            if (roleEntity == null) throw new RoleNotFoundException(roleEntity.Name);

            // Agrega permisos
            foreach (var permName in addPermissions)
            {

                // Busca el permiso en la base de datos
                var permEntity = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permName);
                if (permEntity == null) throw new ApplicationException($"Permission '{permName}' no existe.");
            
                // Verifica si ya existe
                if (!roleEntity.RolePermissions.Any(rp => rp.PermissionId == permEntity.Id))
                {
                    roleEntity.RolePermissions.Add(new RolePermissionEntity
                    {
                        Role = roleEntity,
                        Permission = permEntity
                    });
                }

            }

            // Quita permisos
            foreach (var permName in removePermissions)
            {
                
                var rpToRemove = roleEntity.RolePermissions.FirstOrDefault(rp => rp.Permission.Name == permName);

                if (rpToRemove != null)
                {
                    roleEntity.RolePermissions.Remove(rpToRemove);
                }

            }

            await _context.SaveChangesAsync();

            return RoleMapper.ToDomain(roleEntity);

        }
    }
}
