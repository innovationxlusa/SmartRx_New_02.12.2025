using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.SmartRxInsider;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.SmartRxInsider;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all SmartRx Insider endpoints
    public class SmartRxInsiderController : ControllerBase
    {

        public readonly IMediator _mediator;

        public SmartRxInsiderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("getsmartrxinsiderbyid")]
        [ProducesDefaultResponseType(typeof(List<FolderNodeDTO>))]
        public async Task<IActionResult> GetsmartrxinsiderbyidAsync([FromBody] GetSmartRxMainInsiderQuery query, CancellationToken cancellationToken = default)
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
                        Message = "SmartRx found for this patient!"
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


        [HttpPatch("change-smartrx-doctor-review")]
        [ProducesDefaultResponseType(typeof(SmartRxDoctorDTO))]
        public async Task<ActionResult> EditSmartRxVital([FromBody] ChangeSmartRxDoctorReviewCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Data updated successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Data update failed. Please contact with the system administrator.",
                    StackTrace = null
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.InnerException + "<br/>" + ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("add-smartrx-vital")]
        [ProducesDefaultResponseType(typeof(SmartRxVitalDTO))]
        public async Task<ActionResult> AddSmartRxVital([FromBody] AddSmartRxVitalCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Data saved successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Data save failed. Please contact with the system administrator.",
                    StackTrace = null
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPut("edit-smartrx-vital")]
        [ProducesDefaultResponseType(typeof(SmartRxVitalDTO))]
        public async Task<ActionResult> EditSmartRxVital([FromBody] EditSmartRxSingleVitalCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Data updated successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Data update failed. Please contact with the system administrator.",
                    StackTrace = null
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.InnerException + "<br/>" + ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("medicine-list-to-compare")]      
        [ProducesDefaultResponseType(typeof(MedicineCompareDTO))]
        public async Task<ActionResult> MedicineListToComapre([FromBody] GetSameGenericOtherBrandMedicineQuery command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Comparable data found"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPut("edit-smartrx-medicine-wishlist")]
        [ProducesDefaultResponseType(typeof(SmartRxMedicinesDTO))]
        public async Task<ActionResult> EditSmartRxVital([FromBody] EditSmartRxMedicineWishlistCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                if (result is null)
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not updated"
                    });
                }
                if (result.ApiResponseResult is not null)
                {
                    return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                }
                return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Data updated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.InnerException + "<br/>" + ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet("medicine-faq-list/{medicineId:long}")]
        [ProducesDefaultResponseType(typeof(MedicineCompareDTO))]
        public async Task<ActionResult> MedicineFAQList(long medicineId, CancellationToken cancellationToken = default)
        {
            try
            {
                GetSmartRxInsiderMedicineFAQQuery command = new GetSmartRxInsiderMedicineFAQQuery() { MedicineId = medicineId };
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Comparable data found"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet("vital-faq-list/{vitalId:long}")]
        [ProducesDefaultResponseType(typeof(MedicineCompareDTO))]
        public async Task<ActionResult> VitalFAQList(long vitalId, CancellationToken cancellationToken = default)
        {
            try
            {
                GetSmartRxInsiderVitalFAQQuery command = new GetSmartRxInsiderVitalFAQQuery() { VitalId = vitalId };
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Vital FAQ data found"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("investigation-list-to-compare")]
        [ProducesDefaultResponseType(typeof(InvestigationCompareDTO))]
        public async Task<ActionResult> InvestigationListToComapre([FromBody] GetSmartRxComparedTestListQuery query, CancellationToken cancellationToken = default)
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
                        Message = "Comparable data found"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("investigation-testcenters")]
        [ProducesDefaultResponseType(typeof(TestCentersDTO))]
        public async Task<ActionResult> TestCenterList(CancellationToken cancellationToken = default)
        {
            try
            {
                GetAllTestCentersQuery query = new GetAllTestCentersQuery();
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
                        Message = "Test Centers found!"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("investigation-list-selected-or-recommended")]
        [ProducesDefaultResponseType(typeof(InvestigationCompareDTO))]
        public async Task<ActionResult> InvestigationListSelectdOrRecommended([FromBody] GetSmartRxRecommendedOrSelectedTestCenterListQuery query, CancellationToken cancellationToken = default)
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
                    if (query.IsDoctorRecommended)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                        {
                            Data = result,
                            StatusCode = StatusCodes.Status200OK,
                            Status = "Success",
                            Message = "Recommended test data found"
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                        {
                            Data = result,
                            StatusCode = StatusCodes.Status200OK,
                            Status = "Success",
                            Message = "Selected test data found"
                        });
                    }
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("edit-smartrx-investigation-testcenters")]
        [ProducesDefaultResponseType(typeof(SmartRxMedicinesDTO))]
        public async Task<ActionResult> AddEditSmartRxVInvestigationTestCenters([FromBody] AddEditSmartRxInvestigationTestCenterCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                if (result is null)
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not updated"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Data updated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.InnerException + "<br/>" + ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("edit-smartrx-investigation-wishlist")]
        [ProducesDefaultResponseType(typeof(SmartRxInvestigationWishlistsDTO))]
        public async Task<ActionResult> EditSmartRxInvestigation([FromBody] EditSmartRxInvestigationWishlistCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                if (result is null)
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Data not updated"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Data updated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.InnerException + "<br/>" + ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("investigation-faq-list/{investigationId:long}")]
        [ProducesDefaultResponseType(typeof(InvestigationFAQListDTO))]
        public async Task<ActionResult> InvestigationFAQList(long investigationId, CancellationToken cancellationToken = default)
        {
            try
            {
                GetSmartRxInvestigationFAQuery command = new GetSmartRxInvestigationFAQuery() { InvestigationId = investigationId };
                var result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Investigation FAQ data found"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact with the system administrator.",
                    StackTrace = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Error",
                    Message = "An error occurred. Please contact with system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("smartrx-vital-delete")]
        [ProducesDefaultResponseType(typeof(DeleteDTO))]
        public async Task<ActionResult> SmartRxVitalDelete(DeleteSmartRxVitalCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);

                if (result?.ApiResponseResult != null)
                {
                    return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                }

                if (result != null)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "SmartRx Vital deleted successfully"
                    });
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "No data found. Please contact the system administrator."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Error",
                    Message = "Invalid argument. Please verify your request.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Error",
                    Message = "An unexpected error occurred. Please contact the system administrator.",
                    StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? ex.StackTrace : null
                });
            }

        }

        [HttpPost("GetAllSmartRxWithVitalsByUserId")]
        [ProducesDefaultResponseType(typeof(PaginatedResult<SmartRxWithVitalsDTO>))]
        public async Task<IActionResult> GetAllSmartRxWithVitalsByUserIdAsync([FromBody] GetAllSmartRxWithVitalsByUserIdQuery query, CancellationToken cancellationToken = default)
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
                        Message = "SmartRx with vitals found!"
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

    }
}
