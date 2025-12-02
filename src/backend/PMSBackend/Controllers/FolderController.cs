using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.Folders;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientFolders;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all folder endpoints
    public class FolderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FolderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-folder")]
        [ProducesDefaultResponseType(typeof(UserWiseFolderDTO))]
        public async Task<ActionResult> CreateFolder([FromBody] CreateFolderCommand command, CancellationToken cancellationToken = default)
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
                    return StatusCode(StatusCodes.Status201Created, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status201Created,
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


        [HttpPut("update-folder/{id:long}")]
        [ProducesDefaultResponseType(typeof(UserWiseFolderDTO))]
        public async Task<ActionResult> FolderUpdateAsync(long id, [FromBody] UpdateFolderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid Folder"
                    });
                }
                UserWiseFolderDTO result = await _mediator.Send(command, cancellationToken);
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
                        Message = "Folder updated successfully!"
                    });
                }
                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Folder update failed"
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


        [HttpDelete("delete-folder/{id:long}")]
        [ProducesDefaultResponseType(typeof(long))]
        public async Task<IActionResult> DeleteUser(long id, [FromBody] DeleteFolderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id != command.FolderId)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid Folder"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);
                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }
                    if (result.IsDeleted)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                        {
                            Data = result,
                            StatusCode = StatusCodes.Status200OK,
                            Status = "Success",
                            Message = "Folder deleted successfully"
                        });
                    }
                }

                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Folder delete failed"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetAllFolders/{userId:long}")]
        [ProducesDefaultResponseType(typeof(List<UserWiseFolderDTO>))]
        public async Task<IActionResult> GetAllFolders(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                GetAllFolderListQuery query = new GetAllFolderListQuery() { UserId = userId };
                var result = await _mediator.Send(query, cancellationToken);
                if (result is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "All folders found"
                    });
                }
                return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                {
                    Data = result,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Folder data not found"
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
