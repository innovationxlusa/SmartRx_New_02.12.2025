using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.Role;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Role;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all role endpoints
    public class RoleController : ControllerBase
    {
        public readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> CreateRoleAsync(RoleCreateCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _mediator.Send(command, cancellationToken));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(List<RoleResponseDTO>))]
        public async Task<IActionResult> GetRoleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _mediator.Send(new GetRoleQuery(), cancellationToken));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetSingleRole/{id:long}")]
        [ProducesDefaultResponseType(typeof(RoleResponseDTO))]
        public async Task<IActionResult> GetRoleByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _mediator.Send(new GetRoleByIdQuery() { RoleId = id }, cancellationToken));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetSingleRole/{roleName}")]
        [ProducesDefaultResponseType(typeof(RoleResponseDTO))]
        public async Task<IActionResult> GetRoleByRoleNameAsync(string roleName, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _mediator.Send(new GetRoleByRoleNameQuery() { RoleName = roleName }, cancellationToken));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("Delete/{id:long}")]
        [ProducesDefaultResponseType(typeof(long))]
        public async Task<IActionResult> DeleteRoleAsync(long roleId, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _mediator.Send(new DeleteRoleCommand()
                {
                    RoleId = roleId
                }, cancellationToken));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("Edit/{id:long}")]
        [ProducesDefaultResponseType(typeof(long?))]
        public async Task<ActionResult> EditRole(long id, [FromBody] UpdateRoleCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == command.Id)
                {
                    var result = await _mediator.Send(command);
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
