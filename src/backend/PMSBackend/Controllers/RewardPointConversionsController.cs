using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.Commands.RewardPointConversions;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.RewardPointConversions;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all reward point conversion endpoints
    public class RewardPointConversionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RewardPointConversionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new reward point conversion
        /// </summary>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreateRewardPointConversionAsync([FromBody] CreateRewardPointConversionsCommand command)
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
                        Message = "Reward point conversion details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward point conversion data"
                    });
                }
                if (command.dto.UserId==null || command.dto.UserId<=0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid user id"
                    });
                }
                var result = await _mediator.Send(command);

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
                    Message = $"An error occurred while creating reward point conversion: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing reward point conversion
        /// </summary>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdateRewardPointConversionAsync([FromBody] UpdateRewardPointConversionsCommand command)
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
                        Message = "Reward point conversion details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward point conversion data"
                    });
                }              
                var result = await _mediator.Send(command);

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
                    Message = $"An error occurred while updating reward point conversion: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Convert reward points between different types (Noncashable, Cashable, Money)
        /// </summary>
        [HttpPost("Convert")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> ConvertRewardPointsAsync([FromBody] ConvertRewardPointsCommand command)
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
                        Message = "Conversion request details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid conversion request data"
                    });
                }

                if (command.UserId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid user ID"
                    });
                }

                var result = await _mediator.Send(command);

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
                    Message = $"An error occurred while converting reward points: {ex.Message}"
                });
            }
        }



        /// <summary>
        /// Delete a reward point conversion by ID
        /// </summary>
        [HttpDelete("Delete/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeleteRewardPointConversionAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Reward point conversion details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid reward point conversion data"
                    });
                }
              
                var command = new DeleteRewardPointConversionsCommand 
                { 
                    DeleteDTO = new RewardPointConversionsDeleteDTO { Id = id } 
                };
                var result = await _mediator.Send(command);

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
                    Message = $"An error occurred while deleting reward point conversion: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all reward point conversions
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllRewardPointConversionsAsync()
        {
            try
            {
                var query = new GetAllRewardPointConversionsQuery();
                var result = await _mediator.Send(query);

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
                    Message = $"An error occurred while retrieving reward point conversions: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get reward point conversions by UserId
        /// </summary>
        [HttpGet("GetByUserId")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardPointConversionsByUserIdAsync([FromQuery] long userId)
        {
            try
            {
                var query = new GetRewardPointConversionsByUserIdQuery
                {
                    UserId = userId
                };

                var result = await _mediator.Send(query);

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
                    Message = $"An error occurred while retrieving reward point conversions for user {userId}: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get reward point conversions summary by UserId - shows total converted amounts by conversion types
        /// </summary>
        [HttpGet("GetSummary")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardPointConversionsSummaryAsync([FromQuery] long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid user ID"
                    });
                }

                var query = new GetRewardPointConversionsSummaryQuery
                {
                    UserId = userId
                };

                var result = await _mediator.Send(query);

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
                    Message = $"An error occurred while retrieving reward point conversions summary for user {userId}: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get reward details by UserId - shows all data from PatientReward and RewardConversion as a list
        /// </summary>
        [HttpGet("RewardDetails")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetRewardDetailsAsync([FromQuery] long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid user ID"
                    });
                }

                var query = new GetRewardDetailsQuery
                {
                    UserId = userId
                };

                var result = await _mediator.Send(query);

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
                    Message = $"An error occurred while retrieving reward details for user {userId}: {ex.Message}"
                });
            }
        }
    }
}
