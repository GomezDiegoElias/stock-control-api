using org.pos.software.Domain.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IUserRepository
    {
        public Task<List<User>> FindAll();
        public Task<User> FindById(int id);
        public Task<User> Save(User user);
    }
}
