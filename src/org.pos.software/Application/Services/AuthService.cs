using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using org.pos.software.Application.InPort;
using org.pos.software.Configuration;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Utils;

namespace org.pos.software.Application.Services
{
    public class AuthService : IAuthService
    {

        //private readonly MySqlUserRepository _userRepository;
        //private readonly MySqlRoleRepository _roleRepository;
        //private readonly JwtConfigDto _jwtConfig;

        //public AuthService(JwtConfigDto jwtConfig, MySqlUserRepository userRepository, MySqlRoleRepository roleRepository)
        //{
        //    _jwtConfig = jwtConfig;
        //    _userRepository = userRepository;
        //    _roleRepository = roleRepository;
        //}

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtConfigDto _jwtConfig;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtConfigDto jwtConfig)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtConfig = jwtConfig;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            
            // Verificaciones
            var user = await _userRepository.FindByEmail(request.Email);

            if (user == null) throw new UnauthorizedAccessException("Invalid credentials");

            bool passwordValid = PasswordUtils.VerifyPassword(request.Password, user.Hash, user.Salt);

            if (!passwordValid) throw new UnauthorizedAccessException("Invalid credentials");


            // Generacion del token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);


            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim("role", user.Role.Name)
            };

            // Agrega permisos como claims individuales
            foreach (var perm in user.Role.Permissions)
            {
                claims.Add(new Claim("permission", perm));
            }

            // propiedades del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token); // token generado

            // respuesta del enpoint
            return new AuthResponse(
                Message: "Login successful",
                AccessToken: tokenString, // muestra el token generado
                RefreshToken: "Proximamente" // Implementar refresh token si es necesario
            );

        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {

            // varificaciones por email y dni
            if (await _userRepository.FindByEmail(request.Email) != null) 
                throw new ApplicationException("Email already exists");

            if (await _userRepository.FindByDni(request.Dni) != null) 
                throw new ApplicationException("DNI already exists");

            string salt = PasswordUtils.GenerateRandomSalt();
            string hashedPassword = PasswordUtils.HashPasswordWithSalt(request.Password, salt);

            // Creacion de un nuevo usuario usando el patron builder

            // Busca si existe el rol
            var roleEntity = await _roleRepository.FindByName("PRESUPUESTISTA");
            if (roleEntity == null)
                throw new ApplicationException("Role PRESUPUESTISTA does not exist");

            var newUser = User.Builder()
                .Dni(request.Dni)
                .Email(request.Email)
                .FirstName(request.FirstName)
                .Role(Role.PRESUPUESTISTA)
                .Status(Status.ACTIVE)
                .Hash(hashedPassword)
                .Salt(salt)
                .Build();

            // Genera el id personalizado si no lo tiene
            if (string.IsNullOrEmpty(newUser.Id)) newUser.Id = User.GenerateId();

            // Persiste en base de datos
            var createdUser = await _userRepository.Save(newUser);

            // ???? Logearse al momento de REGISTRAR ????

            // respuesta del endpoint
            AuthResponse response = new AuthResponse(
                Message: "User created successfully",
                AccessToken: "",
                RefreshToken: ""
            );

            return response;

        }

    }
}
