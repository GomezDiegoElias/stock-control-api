using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;

namespace org.pos.software.Application.Services
{
    public class UserService : IUserService
    {

        // Inyeccion del repositorio

        // SqlServer repository
        //private readonly UserRepository _userRepository;

        //public UserService(UserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        // MySql repository
        private readonly MySqlUserRepository _userRepository;

        public UserService(MySqlUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Supabase repository
        //private readonly SupabaseUserRepository _userRepository; 

        //public UserService(SupabaseUserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        // metodos
        public async Task<List<User>> FindAllUsers()
        {
            return await _userRepository.FindAll();
        }

        public async Task<User> FindById(int id)
        {
            return await _userRepository.FindById(id);
        }

    }
}
