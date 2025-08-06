using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.Repositories;

namespace org.pos.software.Application.Services
{
    public class UserService : IUserService
    {

        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<User>> FindAllUsers()
        {
            return _userRepository.FindAll();
        }
    }
}
