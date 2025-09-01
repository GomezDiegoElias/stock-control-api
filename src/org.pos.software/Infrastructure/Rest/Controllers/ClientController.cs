using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("/api/v1/clients")]
    public class ClientController
    {

        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<PaginatedResponse<ClientApiResponse>>>> FindAllClients(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5
        )
        {
            return null;
        }

        [AllowAnonymous]
        [HttpGet("{dni:long}")]
        public async Task<ActionResult<StandardResponse<ClientApiResponse>>> FindClientByDni()
        {
            return null;
        }

        // post -> save client
        // put -> update client
        // delete -> delete client

    }
}
