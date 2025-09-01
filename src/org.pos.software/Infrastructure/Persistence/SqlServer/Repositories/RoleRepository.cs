using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
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

    }
}
