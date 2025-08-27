using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }

    }
}
