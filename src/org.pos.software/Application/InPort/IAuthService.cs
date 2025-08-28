using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Application.InPort
{
    public interface IAuthService
    {
        public Task<AuthResponse> Register(RegisterRequest request);
        public Task<AuthResponse> Login(LoginRequest request);
    }
}
