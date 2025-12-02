using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.SmartRxOtherExpense;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.Queries.SmartRxOtherExpense;


namespace PMSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all SmartRx other expense endpoints
    public class SmartRxOtherExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmartRxOtherExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add a new patient other expense
        /// </summary>
        /// <param name="command">The add patient other expense command</param>
        /// <returns>Created patient other expense</returns>
        [HttpPost("AddSmartRxOtherExpense")]
        public async Task<IActionResult> AddPatientOtherExpense([FromBody] AddSmartRxOtherExpenseCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
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
                        Message = "SmartRx saved for this patient!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while adding patient other expense: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Update an existing patient other expense
        /// </summary>
        /// <param name="command">The edit patient other expense command</param>
        /// <returns>Updated patient other expense</returns>
        [HttpPut("EditSmartRxOtherExpense")]
        public async Task<IActionResult> EditPatientOtherExpense([FromBody] EditPatientOtherExpenseCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

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
                        Message = "SmartRx updated for this patient!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while updating patient other expense: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Delete a patient other expense
        /// </summary>
        /// <param name="id">The ID of the patient other expense to delete</param>
        /// <param name="loginUserId">The ID of the user performing the deletion</param>
        /// <returns>Success status</returns>
        [HttpDelete("DeleteSmartRxOtherExpense/{id}")]
        public async Task<IActionResult> DeletePatientOtherExpense(long id, [FromQuery] long loginUserId)
        {
            try
            {
                var command = new DeletePatientOtherExpenseCommand
                {
                    Id = id,
                    LoginUserId = loginUserId
                };

                var result = await _mediator.Send(command);
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
                        Message = "SmartRx deleted for this patient!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while deleting patient other expense: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get SmartRx other expenses with optional filtering
        /// </summary>
        /// <param name="smartRxMasterId">Optional: Get by SmartRx Master ID</param>
        /// <param name="patientId">Optional: Get by Patient ID</param>
        /// <param name="prescriptionId">Optional: Get by Prescription ID</param>
        /// <returns>List of SmartRx other expenses</returns>
        [HttpGet("GetSmartRxOtherExpenses")]
        public async Task<IActionResult> GetSmartRxOtherExpenses(
            [FromQuery] long? smartRxMasterId = null,
            [FromQuery] long? patientId = null,
            [FromQuery] long? prescriptionId = null)
        {
            try
            {
                var query = new GetSmartRxOtherExpensesQuery
                {
                    SmartRxMasterId = smartRxMasterId,
                    PatientId = patientId,
                    PrescriptionId = prescriptionId
                };

                var result = await _mediator.Send(query);

                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result.Data,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "SmartRx other expenses retrieved successfully!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while getting SmartRx other expenses: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get a patient other expense by ID
        /// </summary>
        /// <param name="id">The ID of the patient other expense</param>
        /// <returns>Patient other expense details</returns>
        [HttpGet("GetSmartRxOtherExpenseById/{id}")]
        public async Task<IActionResult> GetPatientOtherExpenseById(long id)
        {
            try
            {
                var query = new GetPatientOtherExpenseByIdQuery
                {
                    Id = id
                };

                var result = await _mediator.Send(query);

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
                        Message = "Patient other expense found!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while getting patient other expense: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get all patient other expenses by SmartRx Master ID (legacy endpoint)
        /// </summary>
        /// <param name="smartRxMasterId">The SmartRx Master ID</param>
        /// <returns>List of patient other expenses</returns>
        [HttpGet("GetSmartRxOtherExpenseBySmartRxId/{smartRxMasterId}")]
        public async Task<IActionResult> GetPatientOtherExpensesBySmartRxMasterId(long smartRxMasterId)
        {
            try
            {
                var query = new GetSmartRxOtherExpensesQuery
                {
                    SmartRxMasterId = smartRxMasterId
                };

                var result = await _mediator.Send(query);

                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result.Data,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "SmartRx other expenses retrieved successfully!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while getting patient other expenses: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get all patient other expenses by Prescription ID (legacy endpoint)
        /// </summary>
        /// <param name="prescriptionId">The Prescription ID</param>
        /// <returns>List of patient other expenses</returns>
        [HttpGet("GetSmartRxOtherExpenseByPrescriptionId/{prescriptionId}")]
        public async Task<IActionResult> GetPatientOtherExpensesByPrescriptionId(long prescriptionId)
        {
            try
            {
                var query = new GetSmartRxOtherExpensesQuery
                {
                    PrescriptionId = prescriptionId
                };

                var result = await _mediator.Send(query);

                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result.Data,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "SmartRx other expenses retrieved successfully!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while getting patient other expenses: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Get all patient other expenses by Patient ID (legacy endpoint)
        /// </summary>
        /// <param name="patientId">The Patient ID</param>
        /// <returns>List of patient other expenses</returns>
        [HttpGet("GetSmartRxOtherExpenseByPatientId/{patientId}")]
        public async Task<IActionResult> GetPatientOtherExpensesByPatientId(long patientId)
        {
            try
            {
                var query = new GetSmartRxOtherExpensesQuery
                {
                    PatientId = patientId
                };

                var result = await _mediator.Send(query);

                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result.Data,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "SmartRx other expenses retrieved successfully!"
                    });
                }
                else
                {
                    return StatusCode(result.ApiResponseResult?.StatusCode ?? 500, result.ApiResponseResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 400,
                    Status = "Failed",
                    Message = "An error occurred while getting patient other expenses: " + ex.Message
                });
            }
        }
    }
}
