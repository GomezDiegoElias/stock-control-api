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

        // Retorna todos los roles, con opcion de incluir permisos
        public async Task<List<Role>> FindAll(bool includePermissions = false)
        {
            // El 'IQueryable' permite construir consultas dinamicamente
            // Si se usara 'DbSet' directamente, se ejecutaria la consulta inmediatamente
            // y no se podria agregar condiciones adicionales como 'Include'
            // Por eso se usa 'IQueryable' para construir la consulta y luego ejecutarla con 'ToListAsync'
            IQueryable<RoleEntity> query = _context.Roles;

            // Si se indica, incluye los permisos asociados a cada rol
            if (includePermissions)
            {
                query = query
                    .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
            }

            // Ejecuta la consulta y obtiene la lista de entidades
            var entities = await query.ToListAsync();

            // Mapea las entidades a objetos de dominio y retorna la lista
            return entities.Select(r => RoleMapper.ToDomain(r)).ToList();

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
