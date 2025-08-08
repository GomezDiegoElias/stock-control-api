using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;

namespace org.pos.software.Application.Services
{
    public class UserService : IUserService
    {

        private readonly UserRepository _userRepository; // SqlServer repository
        //private readonly SupabaseUserRepository _userRepository; // Supabase repository

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //public UserService(SupabaseUserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        public async Task<List<User>> FindAllUsers()
        {
            return await _userRepository.FindAll();
        }

        //public List<User> FindAllUsers()
        //{
        //    return _userRepository.FindAll();
        //}

        public async Task<User> FindById(int id)
        {
            return await _userRepository.FindById(id);
        }

    }
}
