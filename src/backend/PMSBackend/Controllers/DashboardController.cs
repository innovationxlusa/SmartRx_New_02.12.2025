using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Dashboard;

namespace PMSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all dashboard endpoints
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard-summary")]
        [ProducesDefaultResponseType(typeof(DashboardSummaryDTO))]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] long userId, [FromQuery] long? patientId = null)
        {
            try
            {
                var result = await _mediator.Send(new GetDashboardSummaryQuery { UserId = userId, PatientId = patientId });
                return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Dashboard summary retrieved"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Failed",
                    Message = "An error occurred while retrieving dashboard summary: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get dashboard counts including prescription count, active patients, active doctors, and patient count for specific vital
        /// </summary>
        /// <param name="userId">Required user ID to filter data</param>
        /// <param name="patientId">Optional patient ID to filter data for specific patient</param>
        /// <returns>Dashboard counts data</returns>
        [HttpGet("dashboard-counts")]
        public async Task<ActionResult<DashboardCountsDTO>> GetDashboardCounts([FromQuery] long userId, [FromQuery] long? patientId = null)
        {
            try
            {
                var query = new GetDashboardCountsQuery
                {
                    UserId = userId,
                    PatientId = patientId
                };

                var result = await _mediator.Send(query);

                if (result.ApiResponseResult?.StatusCode == StatusCodes.Status200OK)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? StatusCodes.Status500InternalServerError, result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DashboardCountsDTO
                {
                    ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Status = "Failed",
                        Message = "An error occurred while retrieving dashboard counts: " + ex.Message
                    }
                });
            }
        }
    }
}