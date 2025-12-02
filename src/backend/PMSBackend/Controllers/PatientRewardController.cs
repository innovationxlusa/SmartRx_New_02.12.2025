using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.PatientReward;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.PatientReward;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all patient reward endpoints
    public class PatientRewardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientRewardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new patient reward entry
        /// </summary>
        [HttpPost("CreatePatientReward")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> CreatePatientRewardAsync([FromBody] CreatePatientRewardCommand command)
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
                        Message = "Patient reward details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid patient reward data"
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
                    Message = $"An error occurred while creating patient reward: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing patient reward entry
        /// </summary>
        [HttpPut("UpdatePatientReward")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> UpdatePatientRewardAsync([FromBody] UpdatePatientRewardCommand command)
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
                        Message = "Patient reward details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid patient reward data"
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
                    Message = $"An error occurred while updating patient reward: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a patient reward entry by ID
        /// </summary>
        [HttpDelete("DeletePatientReward/{id}")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> DeletePatientRewardAsync(long id)
        {
            try
            {
                var command = new DeletePatientRewardCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Patient reward with ID {id} not found"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Patient reward deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while deleting patient reward: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get patient rewards by UserId and PatientId with pagination (patientId is optional)
        /// </summary>
        [HttpPost("GetPatientRewardsByUserIdAndPatientId")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetPatientRewardsByUserIdAndPatientIdAsync([FromBody] GetPatientRewardsByUserIdAndPatientIdQuery query)
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
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving patient rewards: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get patient rewards summary (total points and money) by UserId and PatientId (patientId is optional)
        /// </summary>
        [HttpGet("GetPatientRewardsSummary")]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetPatientRewardsSummaryAsync(
            [FromQuery] long userId,
            [FromQuery] long? patientId = null)
        {
            try
            {
                var query = new GetPatientRewardsSummaryQuery
                {
                    UserId = userId,
                    PatientId = patientId
                };

                var result = await _mediator.Send(query);

                if (result is null)
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = "No rewards found for this user and patient"
                    });
                }

                return Ok(new ApiResponseResult
                {
                    Data = result,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Patient rewards summary retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving patient rewards summary: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get patient rewards by UserId only with pagination
        /// </summary>
        //[HttpGet("GetPatientRewardsByUserId")]
        //[ProducesDefaultResponseType(typeof(ApiResponseResult))]
        //public async Task<IActionResult> GetPatientRewardsByUserIdAsync(
        //    [FromQuery] long userId,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 10,
        //    [FromQuery] string? sortBy = "CreatedDate",
        //    [FromQuery] string? sortDirection = "desc")
        //{
        //    try
        //    {
        //        var query = new GetPatientRewardsByUserIdQuery
        //        {
        //            UserId = userId,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            SortBy = sortBy,
        //            SortDirection = sortDirection
        //        };

        //        var result = await _mediator.Send(query);

        //        return Ok(new ApiResponseResult
        //        {
        //            Data = result,
        //            StatusCode = 200,
        //            Status = "Success",
        //            Message = "Patient rewards retrieved successfully"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponseResult
        //        {
        //            Data = null,
        //            StatusCode = 500,
        //            Status = "Error",
        //            Message = $"An error occurred while retrieving patient rewards: {ex.Message}"
        //        });
        //    }
        //}
    }
}

