using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.User;
using PMSBackend.Application.Commands.User.Update;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Validation;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.User;


namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all user endpoints
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("throw")]
        public IActionResult ThrowError()
        {
            throw new Exception("Unhandled exception!");
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                var validationResult = this.ValidateRequest(command);
                if (validationResult != null) return (ActionResult)validationResult;

                // Additional business validation
                var emailResult = this.ValidateEmail(command.Email);
                if (emailResult != null) return (ActionResult)emailResult;

                var phoneResult = this.ValidatePhoneNumber(command.MobileNo);
                if (phoneResult != null) return (ActionResult)phoneResult;

                var result = await _mediator.Send(command, cancellationToken);
                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return this.CreateSuccessResponse(result, StatusCodes.Status201Created, "User created successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "Failed to create user");
                }
            }
            catch (ArgumentException ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status400BadRequest, "Invalid arguments provided", ex);
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while creating user", ex);
            }
        }


        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(List<UserDetailsResponseDTO>))]
        public async Task<IActionResult> GetAllUserAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersDetailsQuery(), cancellationToken);
                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "Users retrieved successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status404NotFound, "No users found");
                }
            }
            catch (ArgumentException ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status400BadRequest, "Invalid arguments provided", ex);
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users", ex);
            }

        }

        [HttpDelete("Delete/{userId:long}")]
        [ProducesDefaultResponseType(typeof(long))]
        public async Task<IActionResult> DeleteUser(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Parameter validation
                var idCheckResult = this.ValidateId(userId, "User ID");
                if (idCheckResult != null) return idCheckResult;

                var result = await _mediator.Send(new DeleteUserCommand() { Id = userId }, cancellationToken);
                if (result)
                {
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "User deleted successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status404NotFound, "User not found or could not be deleted");
                }
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while deleting user", ex);
            }
        }

        [HttpGet("GetUserDetails/{userId:long}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetUserDetails(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Parameter validation
                var idCheckResult = this.ValidateId(userId, "User ID");
                if (idCheckResult != null) return idCheckResult;

                var result = await _mediator.Send(new GetUserDetailsQuery() { UserId = userId }, cancellationToken);
                if (result != null)
                {
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "User details retrieved successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status404NotFound, "User not found");
                }
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving user details", ex);
            }
        }

        [HttpGet("GetUserDetails/{userName}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetUserDetailsByUserName(string userName, CancellationToken cancellationToken = default)
        {
            try
            {
                // Parameter validation
                var userNameResult = this.ValidateString(userName, "User Name");
                if (userNameResult != null) return userNameResult;

                var result = await _mediator.Send(new GetUserDetailsByUserNameQuery() { UserName = userName }, cancellationToken);
                if (result != null)
                {
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "User details retrieved successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status404NotFound, "User not found");
                }
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving user details", ex);
            }
        }

        [HttpPost("AssignRoles")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> AssignRoles(AssignUsersRoleCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                var validationResult = this.ValidateRequest(command);
                if (validationResult != null) return (ActionResult)validationResult;

                var result = await _mediator.Send(command, cancellationToken);
                if (result > 0)
                {
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "Roles assigned successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status400BadRequest, "Failed to assign roles");
                }
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred while assigning roles", ex);
            }
        }

        [HttpPost("AssignRolesToExternalUser")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> AssignRolesToExternalUser(AssignUsersRoleCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[HttpPut("EditUserRoles")]
        //[ProducesDefaultResponseType(typeof(int))]

        //public async Task<ActionResult> EditUserRoles(UpdateUserRolesCommand command)
        //{
        //    try
        //    {
        //        var result = await _mediator.Send(command);
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }           
        //}

        [HttpGet("GetAllUserDetails")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetAllUserDetails(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersDetailsQuery(), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("EditUserProfile/{id:long}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> EditUserProfile(long id, [FromBody] EditUserProfileCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == command.Id)
                {
                    var result = await _mediator.Send(command, cancellationToken);
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
