using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.Ports;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Persistence.Supabase.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Request;
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
        public async Task<ActionResult<StandardResponse<List<UserApiResponse>>>> GetAllUsers()
        {

            List<User> users = await userService.FindAllUsers();
            List<UserApiResponse> response = users.Select(user => UserMapper.ToResponse(user)).ToList();

            return Ok(new StandardResponse<List<UserApiResponse>>(
                Success: true,
                Message: "Users retrieved successfully",
                Data: response
            ));

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> GetUserById(int id)
        {

            try
            {
                User user = await userService.FindById(id);
                UserApiResponse response = UserMapper.ToResponse(user);
                return Ok(new StandardResponse<UserApiResponse>(
                    Success: true,
                    Message: "User retrieved successfully",
                    Data: response
                ));
            }
            catch (Exception ex)
            {
                return NotFound(new StandardResponse<UserApiResponse>(
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Error: new ErrorDetails(
                            Message: "User with the specified ID does not exist.",
                            Details: $"Someting went wrong. {ex.Message}",
                            Path: HttpContext.Request.Path
                        ),
                    Status: 404
                ));
            }

        }

        [HttpPost]
        public async Task<ActionResult<StandardResponse<UserApiResponse>>> CreateUser([FromBody] UserApiRequest request)
        {

            User user = new User
            {
                FirstName = request.Firstname,
                Country = request.Country
            };
            User createdUser = await userService.CreateUser(user);
            UserApiResponse response = UserMapper.ToResponse(createdUser);
            return CreatedAtAction(
                nameof(GetUserById),
                new { id = createdUser.Id },
                new StandardResponse<UserApiResponse>(
                    Success: true,
                    Message: "User created successfully",
                    Data: response,
                    Status: 201
                )
            );

        }

        // //////////////////////////////////////////

    }
}
