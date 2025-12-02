using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.Reward;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.Reward;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all reward endpoints
    public class RewardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RewardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new reward configuration
        /// </summary>
        [HttpPost("CreateReward")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateRewardAsync([FromBody] CreateRewardCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Reward details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                if (result.ApiResponseResult?.StatusCode != StatusCodes.Status200OK)
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
                    Message = $"An error occurred while creating reward: {ex.Message}"
                });
            }
        }

     
        [HttpPut("UpdateReward")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateRewardAsync([FromBody] UpdateRewardCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Reward details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

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
                    Message = $"An error occurred while updating reward: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a reward configuration by ID
        /// </summary>
        [HttpDelete("DeleteReward/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteRewardAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteRewardCommand { Id = id };
                var result = await _mediator.Send(command, cancellationToken);

                if (!result)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Reward with ID {id} not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while deleting reward: {ex.Message}"
                });
            }
        }

       
        [HttpGet("GetRewardById/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetRewardByIdQuery { Id = id };
                var result = await _mediator.Send(query, cancellationToken);

                if (result == null)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Reward with ID {id} not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward: {ex.Message}"
                });
            }
        }

       
        [HttpGet("GetAllRewards")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllRewardsAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedDate",
            [FromQuery] string? sortDirection = "desc",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllRewardsQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _mediator.Send(query, cancellationToken);

                if(result==null || result!.Data.Count<=0)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"No reward found"
                    });
                }
                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Rewards retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving rewards: {ex.Message}"
                });
            }
        }
      
        [HttpGet("GetRewardsByUserActivityId/{userActivityId}")] 
        public async Task<IActionResult> GetRewardsByUserActivityIdAsync(long userActivityId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (userActivityId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid UserActivityId"
                    });
                }

                var query = new GetRewardsByUserActivityIdQuery { UserActivityId = userActivityId };
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
                    Message = $"An error occurred while retrieving rewards by UserActivityId: {ex.Message}"
                });
            }
        }
    }
}

