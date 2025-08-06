using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.Entities;
using org.pos.software.Infrastructure.Persistence.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.Repositories
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


    }
}
