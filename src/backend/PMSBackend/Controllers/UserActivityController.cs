using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.UserActivity;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.UserActivity;
using System;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    /// <summary>
    /// Controller for managing user activities in the SmartRx system
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all user activity endpoints
    public class UserActivityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new user activity
        /// </summary>
        /// <param name="command">The user activity creation command containing ActivityName, ActivityPoint, Remarks, and UserId</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the created user activity with generated ActivityCode</returns>
        /// <response code="200">User activity created successfully</response>
        /// <response code="400">Invalid request data or validation failed</response>
        /// <response code="409">A user activity with the same name already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateUserActivityAsync([FromBody] CreateUserActivityCommand command, CancellationToken cancellationToken = default)
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
                        Message = "User activity details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid user activity data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                if (result.ApiResponseResult?.StatusCode != 200)
                {
                    return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
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
                    Message = $"An error occurred while creating user activity: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all user activities with pagination and sorting
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <param name="sortBy">Field to sort by: ActivityCode, ActivityName, ActivityPoint, or CreatedDate (default: ActivityCode)</param>
        /// <param name="sortDirection">Sort direction: asc or desc (default: asc)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a paginated list of user activities</returns>
        /// <response code="200">User activities retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllUserActivitiesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = "ActivityCode", [FromQuery] string? sortDirection = "asc", CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllUserActivitiesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _mediator.Send(query, cancellationToken);

                if (result.StatusCode != 200)
                {
                    return StatusCode(result.StatusCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving user activities: {ex.Message}"
                });
            }
        }
    }
}

