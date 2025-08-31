using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthController(
            IAuthService authService, 
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator
        )
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<StandardResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
        {

            // Validacion del request
            var validationResult = await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                var errors = new ErrorDetails(400, "Validation failed", HttpContext.Request.Path, validationErrors);
                return new StandardResponse<AuthResponse>(false, "Something went wrong", null, errors, 400);
            }

            var registerResponse = await _authService.Register(request);

            var response = new StandardResponse<AuthResponse>(true, "Register successfully", registerResponse);

            return Ok(response);

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<StandardResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
        {

            // Validacion del request
            var validationResult = await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                var errors = new ErrorDetails(400, "Validation failed", HttpContext.Request.Path, validationErrors);
                return new StandardResponse<AuthResponse>(false, "Something went wrong", null, errors, 400);
            }

            var loginResponse = await _authService.Login(request);

            var response = new StandardResponse<AuthResponse>(true, "Login successfully", loginResponse);

            return Ok(response);

        }


    }
}
