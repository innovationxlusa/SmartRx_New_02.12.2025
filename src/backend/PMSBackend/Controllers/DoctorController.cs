using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.DoctorProfile;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all doctor endpoints
    public class DoctorController : ControllerBase
    {
        public readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetDoctorDetialsById")]
        [ProducesDefaultResponseType(typeof(DoctorProfileDTO))]
        public async Task<IActionResult> GetPatientProfileDetialsAsync([FromBody] GetDoctorProfileByIdQuery query, CancellationToken cancellationToken = default)
        {
            try
            {

                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
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
                        Message = "Doctor profile details found!"
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
                throw;
            }
        }

        [HttpPost("GetDoctorProfilesByUserId")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status417ExpectationFailed)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetDoctorProfilesByUserIdAsync([FromBody] GetDoctorProfilesByUserIdQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Doctor profiles found!"
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
            catch (Exception)
            {
                throw;
            }
        }




    }
}

