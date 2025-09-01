using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Persistence.MySql.Repositories
{
    public class MySqlUserRepository : IUserRepository
    {

        private readonly MySqlDbContext _context;

        public MySqlUserRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<User>> FindAll(int pageIndex, int pageSize)
        {
            
            //var entities = await _context.Users.ToListAsync();

            //var entities = await _context.Users
            //    .Include(u => u.Role)                // trae el rol
            //    .ThenInclude(r => r.RolePermissions) // trae los permisos
            //    .ThenInclude(rp => rp.Permission)    // trae los permisos individuales
            //    .ToListAsync();

            //List<User> users = entities.Select(MySqlUserMapper.ToDomain).ToList();
            return null;

        }

        public async Task<User?> FindByDni(long dni)
        {
        
            var entity = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == dni);

            return entity == null ? null : MySqlUserMapper.ToDomain(entity);
        
        }

        public async Task<User?> FindByEmail(string email)
        {
            var entity = await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);

            return entity == null ? null : MySqlUserMapper.ToDomain(entity);
        }


        public async Task<User> Save(User user)
        {

            // Obtiene RoleEntity según el Role de Domain
            var roleEntity = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == user.Role.Name);

            if (roleEntity == null)
                throw new ApplicationException($"Role {user.Role.Name} does not exist");

            var entity = MySqlUserMapper.ToEntity(user, roleEntity);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return MySqlUserMapper.ToDomain(entity);

        }
    }
}
