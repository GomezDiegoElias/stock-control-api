using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;
using org.pos.software.Utils;

namespace org.pos.software.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<PaginatedResponse<User>> FindAllUsers(int pageIndex, int pageSize)
        {
            return await _userRepository.FindAll(pageIndex, pageSize);
        }

        public Task<User?> FindByDni(long dni)
        {
            return _userRepository.FindByDni(dni);
        }

        public async Task<User> SaveCustomUser(UserApiRequest request)
        {

            if (await _userRepository.FindByEmail(request.Email) != null)
                throw new UserNotFoundException($"Correo electronico '{request.Email}' ya existe");

            if (await _userRepository.FindByDni(request.Dni) != null)
                throw new UserNotFoundException($"Numero de DNI '{request.Dni}' ya existe");

            string salt = PasswordUtils.GenerateRandomSalt();
            string hashedPassword = PasswordUtils.HashPasswordWithSalt(request.Password, salt);

            var roleEntity = await _roleRepository.FindByName(request.Role);
            if (roleEntity == null)
                throw new RoleNotFoundException(request.Role);

            var userCustom = User.Builder()
                .Dni(request.Dni)
                .Email(request.Email)
                .FirstName(request.FirstName)
                .LastName(request.LastName)
                .Role(roleEntity)
                .Status(Status.ACTIVE)
                .Hash(hashedPassword)
                .Salt(salt)
                .Build();

            if (string.IsNullOrEmpty(userCustom.Id)) userCustom.Id = User.GenerateId();

            var savedUser = await _userRepository.Save(userCustom);

            return savedUser;

        }

    }
}
