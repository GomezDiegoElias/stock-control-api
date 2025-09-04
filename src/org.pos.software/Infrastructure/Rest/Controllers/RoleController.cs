using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.pos.software.Application.InPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Rest.Controllers
{

    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/v1/roles")]
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPatch("{roleName}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions(
            string roleName,
            [FromBody] UpdateRolePermissionsRequest request
        )
        {

            var updatedRole = await _roleService.UpdateRolePermissions(
                roleName,
                request.AddPermissions ?? new string[0],
                request.RemovePermissions ?? new string[0]
            );

            return Ok(new
            {
                Success = true,
                Message = $"Permisos del rol '{roleName}' actualizados correctamente.",
                Data = updatedRole
            });

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<StandardResponse<object>>> GetRoles([FromQuery] bool includePermissions = false)
        {
            
            var roles = await _roleService.GetAllRoles(includePermissions);

            // Mapea los roles a respuestas DTO
            var data = roles
                .Select(r => RoleMapper.ToResponse(r, includePermissions))
                .ToList();

            return Ok(new StandardResponse<object>(
                true,
                includePermissions ? "Roles con permisos" : "Roles (solo nombres)",
                data,
                null,
                200
            ));

        }


    }

}
