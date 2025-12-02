using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.RewardRule;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.RewardRule;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all reward rule endpoints
    public class RewardRuleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RewardRuleController(IMediator mediator, ICodeGenerationService codeGenerationService)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new reward rule
        /// </summary>
        [HttpPost("CreateRewardRule")]      
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateRewardRuleAsync([FromBody] CreateRewardRuleCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Reward rule details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward rule data"
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
                    Message = $"An error occurred while creating reward rule: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing reward rule
        /// </summary>
        [HttpPut("UpdateRewardRule")]       
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateRewardRuleAsync([FromBody] UpdateRewardRuleCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Reward rule details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward rule data"
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
                    Message = $"An error occurred while updating reward rule: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a reward rule by ID
        /// </summary>
        [HttpDelete("DeleteRewardRule/{id}")]      
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteRewardRuleAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteRewardRuleCommand { Id = id };
                var result = await _mediator.Send(command, cancellationToken);

                if (!result)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Reward rule with ID {id} not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward rule deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while deleting reward rule: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get a reward rule by ID
        /// </summary>
        [HttpGet("GetRewardRuleById/{id}")]      
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardRuleByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetRewardRuleByIdQuery { Id = id };
                var result = await _mediator.Send(query, cancellationToken);

                if (result == null)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Reward rule with ID {id} not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward rule retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward rule: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get a reward rule by Activity Name
        /// </summary>
        [HttpGet("GetRewardRuleByActivityName/{activityName}")]     
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardRuleByActivityNameAsync(string activityName, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetRewardRuleByActivityNameQuery { ActivityName = activityName };
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
                    Message = $"Reward rule with activity name '{activityName}' not found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward rule: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all reward rules with pagination
        /// </summary>
        [HttpGet("GetAllRewardRules")]      
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllRewardRulesAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedDate",
            [FromQuery] string? sortDirection = "desc",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllRewardRulesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _mediator.Send(query, cancellationToken);

                if (result == null || result.Data.Count <= 0)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = "No reward rules found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward rules retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward rules: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all visible benefits from reward rules (where IsVisibleBenifit=true)
        /// </summary>
        [HttpGet("GetAllVisibleBenefits")]       
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllVisibleBenefitsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllVisibleBenefitsQuery();
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
                    Message = $"An error occurred while retrieving visible benefits: {ex.Message}"
                });
            }
        }
    }
}

