using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository
    {

        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> FindAll()
        {
            List<UserEntity> entities = await _dbContext.Users.ToListAsync();
            List<User> users = entities.Select(UserMapper.ToDomain).ToList();
            return users;
        }

        public async Task<User> FindById(int id)
        {
            var entity = await _dbContext.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
            return UserMapper.ToDomain(entity);
        }

    }
}
