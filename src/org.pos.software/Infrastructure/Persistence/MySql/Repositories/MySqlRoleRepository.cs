using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using Microsoft.EntityFrameworkCore;

namespace org.pos.software.Infrastructure.Persistence.MySql.Repositories
{
    public class MySqlRoleRepository : IRoleRepository
    {

        private readonly MySqlDbContext _context;

        public MySqlRoleRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<RoleEntity> FindByName(string name)
        {
            return await _context.Roles
                            .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                            .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<RoleEntity> Save(RoleEntity role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<List<RoleEntity>> FindAll()
        {
            return await _context.Roles
                            .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                            .ToListAsync();
        }

    }
}
