using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
//using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Mappers;
//using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
// using org.pos.software.Infrastructure.Persistence.Supabase.Mappers;
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

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // falta paginacion, filtros, etc

        // -------------------------
        // GET: /api/v1/users
        // Acceso solo para ADMIN
        // -------------------------
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<List<UserApiResponse>>>> GetAllUsers()
        {

            List<User> users = await userService.FindAllUsers();
            List<UserApiResponse> response = users.Select(user => MySqlUserMapper.ToResponse(user)).ToList();

            return Ok(new StandardResponse<List<UserApiResponse>>(
                Success: true,
                Message: "Users retrieved successfully",
                Data: response
            ));

        }


        // -------------------------
        // GET: /api/v1/users/{dni}
        // Acceso para cualquier rol con permiso READ
        // -------------------------
        [Authorize(Policy = "CanRead")]
        [HttpGet("{dni:long}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> GetUserByDni(long dni)
        {
            try
            {

                User? user = await userService.FindByDni(dni);

                if (user == null)
                {
                    return NotFound(new StandardResponse<UserApiResponse>(
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Error: new ErrorDetails(
                            Message: "Something went wrong.",
                            Details: "User with the specified DNI does not exist.",
                            Path: HttpContext.Request.Path
                        ),
                    Status: 404
                ));
                }

                // validacion en Domain
                if (!user.Can("READ"))
                {
                    return Forbid();
                }

                UserApiResponse response = MySqlUserMapper.ToResponse(user);
                return Ok(new StandardResponse<UserApiResponse>(
                    Success: true,
                    Message: "User retrieved successfully",
                    Data: response
                ));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new StandardResponse<UserApiResponse>(
                    Success: false,
                    Message: "Internal server error",
                    Data: null,
                    Error: new ErrorDetails(
                        Message: "An unexpected error occurred.",
                        Details: ex.Message,
                        Path: HttpContext.Request.Path
                    ),
                    Status: 500
                ));
            }
        
        }

        // //////////////////////////////////////////

    }
}
