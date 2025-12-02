using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.RewardBadge;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.RewardBadge;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all reward badge endpoints
    public class RewardBadgeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RewardBadgeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new reward badge
        /// </summary>
        [HttpPost("CreateRewardBadge")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateRewardBadgeAsync([FromBody] CreateRewardBadgeCommand command)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Reward badge details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward badge data"
                    });
                }

                var result = await _mediator.Send(command);

                if (result.ApiResponseResult?.StatusCode != 200)
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }

                return Ok(result.ApiResponseResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while creating reward badge: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing reward badge
        /// </summary>
        [HttpPut("UpdateRewardBadge")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateRewardBadgeAsync([FromBody] UpdateRewardBadgeCommand command)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Reward badge details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward badge data"
                    });
                }

                var result = await _mediator.Send(command);

                if (result.ApiResponseResult?.StatusCode != 200)
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }

                return Ok(result.ApiResponseResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while updating reward badge: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a reward badge by ID
        /// </summary>
        [HttpDelete("DeleteRewardBadge/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteRewardBadgeAsync(int id)
        {
            try
            {
                var command = new DeleteRewardBadgeCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"This reward badge not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward badge deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while deleting reward badge: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get a reward badge by ID
        /// </summary>
        [HttpGet("GetRewardBadgeById/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardBadgeByIdAsync(int id)
        {
            try
            {
                var query = new GetRewardBadgeByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                if (result != null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Reward badge retrieved successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = $"This reward badge not found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward badge: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all reward badges with pagination
        /// </summary>
        [HttpGet("GetAllRewardBadges")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllRewardBadgesAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "Heirarchy",
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                var query = new GetAllRewardBadgesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _mediator.Send(query);

                if (result is not null)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Patient profiles found!"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Data not found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward badges: {ex.Message}"
                });
            }
        }
    }
}

