using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IUserRepository
    {
        public Task<List<User>> FindAll();
        //public Task<User?> FindById(int id);
        public Task<User?> FindByDni(long dni);
        public Task<User> Save(User user);
        public Task<User?> FindByEmail(string email);
    }
}
