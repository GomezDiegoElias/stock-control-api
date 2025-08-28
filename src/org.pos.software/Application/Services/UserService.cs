using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;

namespace org.pos.software.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> FindAllUsers()
        {
            return await _userRepository.FindAll();
        }

        public Task<User?> FindByDni(long dni)
        {
            return _userRepository.FindByDni(dni);
        }

    }
}
