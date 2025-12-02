using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.BrowseRx;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all browse Rx endpoints
    public class BrowseRxController : ControllerBase
    {
        public readonly IMediator _mediator;

        public BrowseRxController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("getallparentfoldersandfiles")]
        [ProducesDefaultResponseType(typeof(List<FolderNodeDTO>))]
        public async Task<IActionResult> GetBrwoseRxFilesAndFoldersListAsync([FromBody] GetBrowseRxQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                query.CurrentFolderPath = folderPath;
                Console.WriteLine("BrowseRx: Get all parent folder and files ",folderPath);
                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
                {
                    var finalResult = result.Children.Data
                                    .OrderBy(x => x.IsFolder ? 0 : 1)
                                    .ThenByDescending(x => x.CreatedDate);
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Folder and files data found"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not found. Please contact with the system administrator.",
                        StackTrace = null
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get patient prescriptions with pagination and search
        /// </summary>
        /// <param name="query">Search query containing user ID, patient ID, search parameters, and paging information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of patient prescriptions</returns>
        [HttpPost("getpatientprescriptions")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status417ExpectationFailed)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetPatientPrescriptionsAsync([FromBody] GetPatientPrescriptionsQuery query, CancellationToken cancellationToken = default)
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
                        Message = "Patient prescriptions data found"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not found. Please contact with the system administrator.",
                        StackTrace = null
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("getpatientprescriptionsbytype")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status417ExpectationFailed)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetPatientPrescriptionsByTypeAsync([FromBody] PatientPrescriptionByTypeRequestDTO request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (request.PagingSorting == null)
                {
                    request.PagingSorting = new PagingSortingParams();
                }

                var query = new GetPatientPrescriptionsByTypeQuery
                {
                    UserId = request.UserId,
                    PatientId = request.PatientId,
                    PrescriptionType = request.PrescriptionType,
                    PagingSorting = request.PagingSorting
                };

                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = $"Patient {request.PrescriptionType} prescriptions data found"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not found. Please contact with the system administrator.",
                        StackTrace = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
    }
}
