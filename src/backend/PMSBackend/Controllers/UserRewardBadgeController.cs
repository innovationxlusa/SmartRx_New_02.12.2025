using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.UserRewardBadge;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.UserRewardBadge;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRewardBadgeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRewardBadgeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUserRewardBadge")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateUserRewardBadgeAsync(
            [FromBody] CreateUserRewardBadgeCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "User reward badge details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid user reward badge data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                return StatusCode(result.ApiResponseResult?.StatusCode ?? StatusCodes.Status500InternalServerError, result.ApiResponseResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while creating user reward badge: {ex.Message}"
                });
            }
        }

        [HttpPut("UpdateUserRewardBadge")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateUserRewardBadgeAsync(
            [FromBody] UpdateUserRewardBadgeCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "User reward badge details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid user reward badge data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                return StatusCode(result.ApiResponseResult?.StatusCode ?? StatusCodes.Status500InternalServerError, result.ApiResponseResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while updating user reward badge: {ex.Message}"
                });
            }
        }

        [HttpDelete("DeleteUserRewardBadge/{id:long}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteUserRewardBadgeAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteUserRewardBadgeCommand { Id = id };
                var result = await _mediator.Send(command, cancellationToken);

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while deleting user reward badge: {ex.Message}"
                });
            }
        }

        [HttpGet("GetUserRewardBadgeById/{id:long}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetUserRewardBadgeByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetUserRewardBadgeByIdQuery { Id = id };
                var result = await _mediator.Send(query, cancellationToken);

                if (result?.ApiResponseResult is not null)
                {
                    return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "User reward badge not found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving user reward badge: {ex.Message}"
                });
            }
        }

        [HttpGet("GetAllUserRewardBadges")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllUserRewardBadgesAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedDate",
            [FromQuery] string? sortDirection = "desc",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllUserRewardBadgesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _mediator.Send(query, cancellationToken);

                if (result != null && result.Data != null && result.Data.Count > 0)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "User reward badges retrieved successfully"
                    });
                }

                return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "No user reward badges found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving user reward badges: {ex.Message}"
                });
            }
        }

        [HttpGet("GetLatestUserRewardBadge/{userId:int}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetLatestUserRewardBadgeAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetLatestUserRewardBadgeQuery { UserId = userId };
                var result = await _mediator.Send(query, cancellationToken);

                if (result?.ApiResponseResult is not null)
                {
                    return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Latest user reward badge not found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving latest user reward badge: {ex.Message}"
                });
            }
        }
    }
}

