using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.RewardTransaction;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.PatientReward;
using PMSBackend.Application.Queries.RewardTransaction;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RewardTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RewardTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateRewardTransaction")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateRewardTransactionAsync(
            [FromBody] CreateRewardTransactionCommand command,
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
                        Message = "Reward transaction details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid reward transaction data"
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
                    Message = $"An error occurred while creating reward transaction: {ex.Message}"
                });
            }
        }

        [HttpPut("UpdateRewardTransaction")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateRewardTransactionAsync(
            [FromBody] UpdateRewardTransactionCommand command,
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
                        Message = "Reward transaction details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid reward transaction data"
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
                    Message = $"An error occurred while updating reward transaction: {ex.Message}"
                });
            }
        }

        [HttpDelete("DeleteRewardTransaction/{id:long}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteRewardTransactionAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new DeleteRewardTransactionCommand { Id = id };
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
                    Message = $"An error occurred while deleting reward transaction: {ex.Message}"
                });
            }
        }

        [HttpGet("GetRewardTransactionById/{id:long}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardTransactionByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetRewardTransactionByIdQuery { Id = id };
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
                    Message = "Reward transaction not found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transaction: {ex.Message}"
                });
            }
        }

        [HttpGet("GetAllRewardTransactions")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllRewardTransactionsAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedDate",
            [FromQuery] string? sortDirection = "desc",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetAllRewardTransactionsQuery
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
                        Message = "Reward transactions retrieved successfully"
                    });
                }

                return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "No reward transactions found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transactions: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get reward transaction summary by UserId - shows total amounts from RewardTransaction and RewardPointConversions tables
        /// </summary>
        [HttpGet("RewardSummary")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardTransactionSummaryAsync([FromQuery] long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid user ID"
                    });
                }

                var query = new GetRewardTransactionSummaryQuery
                {
                    UserId = userId
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transaction summary for user {userId}: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get reward transaction details by UserId - shows all data from RewardTransaction and RewardPointConversions as a list
        /// </summary>
        [HttpPost("RewardDetails")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardTransactionDetailsAsync(GetPatientRewardsByUserIdAndPatientIdQuery query, CancellationToken cancellationToken = default)
           
        {
            try
            {
                if (query == null)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Request body is required"
                    });
                }

                var result = await _mediator.Send(query);

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Patient rewards retrieved successfully"
                });               
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward transaction details for user {query.UserId}: {ex.Message}"
                });
            }
        }
    
        
    
    
    }
}

