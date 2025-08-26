using org.pos.software.Domain.Entities;

namespace org.pos.software.Application.Ports
{
    public interface IUserService
    {
        public Task<List<User>> FindAllUsers();
        //public List<User> FindAllUsers();
        public Task<User> FindById(int id);
        public Task<User> CreateUser(User user);
    }
}
