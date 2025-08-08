using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Persistence.Supabase.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [ApiController]
    [Route("api/v1/users")]
    public class UserController : Controller
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<UserApiResponse>>> getAllUsers()
        //{

        //    List<User> users = await userService.FindAllUsers();
        //    List<UserApiResponse> response = users.Select(user => UserMapper.ToResponse(user)).ToList();

        //    return Ok(response);

        //}

        [HttpGet]
        public async Task<ActionResult<List<UserApiResponse>>> getAllUsers()
        {

            List<User> users = await userService.FindAllUsers();
            List<UserApiResponse> response = users.Select(user => UserMapper.ToResponse(user)).ToList();

            return Ok(response);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserApiResponse>> getUserById(int id)
        {
            try
            {
                User user = await userService.FindById(id);
                UserApiResponse response = SupabaseUserMapper.ToResponse(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
