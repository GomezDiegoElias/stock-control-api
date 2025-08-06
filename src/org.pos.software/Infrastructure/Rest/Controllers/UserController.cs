using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.Mappers;
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

        [HttpGet]
        public async Task<ActionResult<List<UserApiResponse>>> getAllUsers()
        {

            List<User> users = await userService.FindAllUsers();
            List<UserApiResponse> response = users.Select(user => UserMapper.ToResponse(user)).ToList();

            return Ok(response);

        }

    }
}
