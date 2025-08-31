using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;

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
        //[Authorize(Policy = "CanRead")]
        [AllowAnonymous]
        [HttpGet("{dni:long}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> GetUserByDni(long dni)
        {

            var user = await userService.FindByDni(dni);
            if (user == null) throw new UserNotFoundException(dni.ToString());

            var response = MySqlUserMapper.ToResponse(user);
            return Ok(new StandardResponse<UserApiResponse>(true, "User retrieved successfully", response));

        }

        // //////////////////////////////////////////

    }
}
