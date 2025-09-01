using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<User>> FindAll(int pageIndex, int pageSize)
        {
            return await _context.getUserPagination(pageIndex, pageSize);
        }

        public async Task<User?> FindByDni(long dni)
        {

            var entity = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == dni);

            return entity == null ? null : UserMapper.ToDomain(entity);

        }

        public async Task<User?> FindByEmail(string email)
        {
            var entity = await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);

            return entity == null ? null : UserMapper.ToDomain(entity);
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

            var entity = UserMapper.toEntity(user, roleEntity);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(entity);

        }

    }
}
