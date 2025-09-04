using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IValidator<UserApiRequest> _userValidator;

        public UserController(IUserService userService, IValidator<UserApiRequest> userValidator)
        {
            _userService = userService;
            _userValidator = userValidator;
        }

        // falta paginacion, filtros, etc

        // -------------------------
        // GET: /api/v1/users
        // Acceso solo para ADMIN
        // -------------------------
        //[Authorize(Roles = "ADMIN")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<PaginatedResponse<UserApiResponse>>>> GetAllUsers(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5
        )
        {

            var users = await _userService.FindAllUsers(pageIndex, pageSize);

            var userResponse = users.Items.Select(user => UserMapper.ToResponse(user)).ToList();

            var paginatedResponse = new PaginatedResponse<UserApiResponse>
            {
                Items = userResponse,
                PageIndex = users.PageIndex,
                PageSize = users.PageSize,
                TotalItems = users.TotalItems,
                TotalPages = users.TotalPages
            };

            return Ok(new StandardResponse<PaginatedResponse<UserApiResponse>>(
                Success: true,
                Message: "Usuarios obtenidos exitosamente",
                Data: paginatedResponse
            ));


        }


        // -------------------------
        // GET: /api/v1/users/{dni}
        // Acceso para cualquier rol con permiso READ
        // -------------------------
        //[Authorize(Policy = "CanRead")]
        [AllowAnonymous]
        [HttpGet("{dni:long}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> GetUserByDni(long dni)
        {

            var user = await _userService.FindByDni(dni);
            if (user == null) throw new UserNotFoundException(dni.ToString());

            var response = UserMapper.ToResponse(user);
            return Ok(new StandardResponse<UserApiResponse>(true, "Usuario obtenido exitosamente", response));

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> CreateUser([FromBody] UserApiRequest request)
        {

            var validationResult = await _userValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                var errors = new ErrorDetails(400, "Validacion fallida", HttpContext.Request.Path, validationErrors);
                return new StandardResponse<UserApiResponse>(false, "Ah ocurrido un error", null, errors);
            }

            var newUser = await _userService.SaveCustomUser(request);

            var response = new StandardResponse<UserApiResponse>(
                Success: true,
                Message: "Usuario creado exitosamente",
                Data: UserMapper.ToResponse(newUser)
            );

            return Created(string.Empty, response);

        }

        [AllowAnonymous]
        [HttpDelete("permanent/{dni:long}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> DeleteUserPermanently(long dni)
        {
            var deletedUser = await _userService.DeletePermanent(dni);
            var response = new StandardResponse<UserApiResponse>(
                Success: true,
                Message: "Usuario eliminado permanentemente",
                Data: UserMapper.ToResponse(deletedUser)
            );
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpDelete("{dni:long}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> DeleteUserLogically(long dni)
        {
            var deletedUser = await _userService.DeleteLogic(dni);
            var response = new StandardResponse<UserApiResponse>(
                Success: true,
                Message: "Usuario eliminado",
                Data: UserMapper.ToResponse(deletedUser)
            );
            return Ok(response);
        }

        // //////////////////////////////////////////

    }
}
