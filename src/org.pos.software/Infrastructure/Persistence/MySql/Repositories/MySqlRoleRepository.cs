﻿using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Mappers;

namespace org.pos.software.Infrastructure.Persistence.MySql.Repositories
{
    public class MySqlRoleRepository : IRoleRepository
    {

        private readonly MySqlDbContext _context;

        public MySqlRoleRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<Role> FindByName(string name)
        {
            var entity = await _context.Roles
                            .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                            .FirstOrDefaultAsync(r => r.Name == name);
            return MysSqlRoleMapper.ToDomain(entity);
        }

        public async Task<Role> Save(Role role)
        {
            var entity = MysSqlRoleMapper.toEntity(role);
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return MysSqlRoleMapper.ToDomain(entity);
        }

    }
}
