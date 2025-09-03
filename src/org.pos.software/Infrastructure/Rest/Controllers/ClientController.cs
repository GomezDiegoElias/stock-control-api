using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;
using org.pos.software.Utils.Validations;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("/api/v1/clients")]
    public class ClientController : Controller
    {

        private readonly IClientService _service;
        private readonly ClientValidation _validation;

        public ClientController(IClientService service, ClientValidation validation)
        {
            _service = service;
            _validation = validation;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<PaginatedResponse<ClientApiResponse>>>> FindAllClients(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5
        )
        {

            var clients = await _service.FindAll(pageIndex, pageSize);
            var clientResponse = clients.Items.Select(c => ClientMapper.ToResponse(c)).ToList();

            var paginatedResponse = new PaginatedResponse<ClientApiResponse>
            {
                Items = clientResponse,
                PageIndex = clients.PageIndex,
                PageSize = clients.PageSize,
                TotalItems = clients.TotalItems,
                TotalPages = clients.TotalPages
            };

            return Ok(new StandardResponse<PaginatedResponse<ClientApiResponse>>(
                Success: true,
                Message: "Clientes obtenidos exitosamente",
                Data: paginatedResponse
            ));

        }

        [AllowAnonymous]
        [HttpGet("{dni:long}")]
        public async Task<ActionResult<StandardResponse<ClientApiResponse>>> FindClientByDni(long dni)
        {
         
            var client = await _service.FindByDni(dni);
            if (client == null) throw new ClientNotFoundException(dni.ToString());

            var response = ClientMapper.ToResponse(client);
            return Ok(new StandardResponse<ClientApiResponse>(true, "Cliente obtenido exitosamente", response));

        }

        // post -> save client
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<StandardResponse<ClientApiResponse>>> CreatedClient(
            [FromBody] ClientApiRequest request   
        )
        {

            var validationResult = await _validation.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                var errors = new ErrorDetails(400, "Validation failed", HttpContext.Request.Path, validationErrors);
                return new StandardResponse<ClientApiResponse>(false, "Something went wrong", null, errors, 400);
            }

            var newClient = ClientMapper.ToDomain(request);
            var savedResponse = await _service.Save(newClient);
            var response = ClientMapper.ToResponse(savedResponse);

            return Created(string.Empty, new StandardResponse<ClientApiResponse>(true, "Created client successfully", response, null, 201));

        }


        // put -> update client
        [AllowAnonymous]
        [HttpPut("{dni:long}")]
        public async Task<ActionResult<StandardResponse<ClientApiResponse>>> UpdateClient(
            [FromBody] ClientApiRequest request,
            long dni
        )
        {

            var existingClient = await _service.FindByDni(dni);

            if (existingClient == null) throw new ClientNotFoundException(dni.ToString());

            var validationResult = await _validation.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                var errors = new ErrorDetails(400, "Validacion fallida", HttpContext.Request.Path, validationErrors);
                return new StandardResponse<ClientApiResponse>(false, "Ah ocurrido un error", null, errors, 400);
            }

            var clientToUpdate = ClientMapper.ToDomain(request);
            clientToUpdate.Id = existingClient.Id;

            var updatedClient = await _service.Update(clientToUpdate);
            var response = ClientMapper.ToResponse(updatedClient);

            return Ok(new StandardResponse<ClientApiResponse>(true, "Cliente actualizado exitosamente", response));

        }


        // delete -> delete client

    }
}
