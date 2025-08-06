using org.pos.software.Domain.Entities;

namespace org.pos.software.Application.Ports
{
    public interface IUserService
    {
        public Task<List<User>> FindAllUsers();
    }
}
