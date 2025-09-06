using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/v1/employees")]
    public class EmployeeController : Controller
    {

        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<PaginatedResponse<EmployeeApiResponse>>>> GetAllEmployees (
            [FromQuery] int pageIndex = 1, 
            [FromQuery] int pageSize = 5,
            [FromQuery] int? dni = null,
            [FromQuery] string? firstname = null,
            [FromQuery] string? lastname = null,
            [FromQuery] string? workstation = null
        )
        {

            var employees = await _employeeService.FindAll(pageIndex, pageSize, dni, firstname, lastname, workstation);
            var response = employees.Items.Select(e => EmployeeMapper.ToResponse(e)).ToList();

            var paginatedResponse = new PaginatedResponse<EmployeeApiResponse>
            {
                Items = response,
                PageIndex = employees.PageIndex,
                PageSize = employees.PageSize,
                TotalItems = employees.TotalItems,
                TotalPages = employees.TotalPages
            };

            return Ok(new StandardResponse<PaginatedResponse<EmployeeApiResponse>>(
                Success: true,
                Message: "Empleados obtenidos correctamente",
                Data: paginatedResponse
            ));

        }

    }
}
