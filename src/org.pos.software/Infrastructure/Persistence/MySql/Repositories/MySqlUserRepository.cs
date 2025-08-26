using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Mappers;

namespace org.pos.software.Infrastructure.Persistence.MySql.Repositories
{
    public class MySqlUserRepository : IUserRepository
    {

        private readonly MySqlDbContext _context;

        public MySqlUserRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> FindAll()
        {
            var entities = await _context.Users.ToListAsync();
            List<User> users = entities.Select(MySqlUserMapper.ToDomain).ToList();
            return users;
        }

        public async Task<User> FindById(int id)
        {
            var entity = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found");
            return MySqlUserMapper.ToDomain(entity);
        }

        public async Task<User> Save(User user)
        {
            var entity = MySqlUserMapper.ToEntity(user);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return MySqlUserMapper.ToDomain(entity);
        }
    }
}
