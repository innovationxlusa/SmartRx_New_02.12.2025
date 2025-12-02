using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Vital;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all vital endpoints
    public class VitalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VitalController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("GetVitalsByVitalName")]
        [ProducesDefaultResponseType(typeof(List<VitalDTO>))]
        public async Task<IActionResult> GetAllFolders(GetAllVitalByVitalNameQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(query.VitalName))
                {

                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "No vital found"
                    });
                }

                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
                {
                    var apiResponse = result.Where(data => data.ApiResponseResult != null).FirstOrDefault();
                    if (apiResponse is not null)
                    {
                        return StatusCode(apiResponse.ApiResponseResult.StatusCode, apiResponse.ApiResponseResult);
                    }
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Data found successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Data not found.",
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
