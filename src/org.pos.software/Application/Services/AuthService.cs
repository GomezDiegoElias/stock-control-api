using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using org.pos.software.Application.InPort;
using org.pos.software.Configuration;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Utils;
using System.Security.Claims;

namespace org.pos.software.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly MySqlUserRepository _userRepository;
        private readonly JwtConfigDto _jwtConfig;

        public AuthService(MySqlUserRepository userRepository, JwtConfigDto jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            
            var user = await _userRepository.FindByEmail(request.Email);

            if (user == null) throw new UnauthorizedAccessException("Invalid credentials");

            bool passwordValid = PasswordUtils.VerifyPassword(request.Password, user.Hash, user.Salt);

            if (!passwordValid) throw new UnauthorizedAccessException("Invalid credentials");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim("dni", user.Dni.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", user.Role.ToString()),
                    // Agregar mas claims si es necesario
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponse(
                Message: "Login successful",
                AccessToken: tokenString,
                RefreshToken: "" // Implementar refresh token si es necesario
            );

        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {

            if (await _userRepository.FindByEmail(request.Email) != null) 
                throw new ApplicationException("Email already exists");

            // Validacion por DNI

            string salt = PasswordUtils.GenerateRandomSalt();
            string hashedPassword = PasswordUtils.HashPasswordWithSalt(request.Password, salt);

            var newUser = new User.Builder()
                .Dni(request.Dni)
                .Email(request.Email)
                .FirstName(request.FirstName)
                .Role(Role.PRESUPUESTISTA)
                .Status(Status.ACTIVE)
                .Hash(hashedPassword)
                .Salt(salt)
                .Build();

            newUser.generateId();

            var createdUser = await _userRepository.Save(newUser);

            AuthResponse response = new AuthResponse(
                Message: "User created successfully",
                AccessToken: "",
                RefreshToken: ""
            );

            return response;

        }

    }
}
