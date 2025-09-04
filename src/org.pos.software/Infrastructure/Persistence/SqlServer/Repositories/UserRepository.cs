using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;
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

        public async Task<User> DeleteLogic(long dni)
        {
            
            var user = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == dni);

            if (user == null) throw new UserNotFoundException($"Usuario con DNI {dni} no existe");

            user.Status = Status.DELETED; // Cambio de estado a DELETED

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(user);

        }

        public async Task<User> DeletePermanent(long dni)
        {
           
            var user = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == dni);

            if (user == null) throw new UserNotFoundException($"Usuario con DNI {dni} no existe");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(user);

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
                throw new RoleNotFoundException(user.Role.Name);

            var entity = UserMapper.toEntity(user, roleEntity);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(entity);

        }

        public async Task<User> Update(User user)
        {
            
            var existingUser = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == user.Dni);

            if (existingUser == null) throw new UserNotFoundException($"Usuario con DNI {user.Dni} no existe");

            // Actualiza los campos del usuario existente
            existingUser.Dni = user.Dni;
            existingUser.Email = user.Email;
            existingUser.Hash = user.Hash;
            existingUser.Salt = user.Salt;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Status = user.Status;

            // Buscar el RoleEntity correspondiente
            var roleEntity = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == user.Role.Name);

            if (roleEntity == null) throw new RoleNotFoundException(user.Role.Name);

            existingUser.RoleId = roleEntity.Id;

            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(existingUser);

        }

        public async Task<User> UpdatePartial(User user)
        {
            
            var existingUser = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Dni == user.Dni);

            if (existingUser == null) throw new UserNotFoundException($"Usuario con DNI {user.Dni} no existe");

            // Actualiza solo los campos que no son nulos en el objeto user
            existingUser.Dni = existingUser.Dni;
            existingUser.Email = user.Email;
            existingUser.Hash = user.Hash;
            existingUser.Salt = user.Salt;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Status = user.Status;

            // Buscar el RoleEntity correspondiente
            var roleEntity = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == user.Role.Name);

            if (roleEntity == null) throw new RoleNotFoundException(user.Role.Name);

            existingUser.RoleId = roleEntity.Id;

            await _context.SaveChangesAsync();

            return UserMapper.ToDomain(existingUser);

        }
    }
}
