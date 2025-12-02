using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.PatientProfile;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientProfile;
using PMSBackend.Domain.SharedContract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all patient profile endpoints
    public class PatientProfileController : ControllerBase
    {
        public readonly IMediator _mediator;

        public PatientProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CreatePatientProfile")]
        [ProducesDefaultResponseType(typeof(PatientWithRelativesDTO))]
        public async Task<IActionResult> CreatePatientProfileAsync([FromForm] CreatePatientProfileCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Patient details not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid data"
                    });
                }

                // Handle profile photo upload if provided
                if (command.PatientDetails?.ProfilePhoto is not null && command.PatientDetails.ProfilePhoto.Length > 0)
                {
                    var checkFileTypeMatched = CheckFileFormatMatched(command.PatientDetails.ProfilePhoto);
                    if (checkFileTypeMatched is not null)
                        return checkFileTypeMatched;

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    using var stream = command.PatientDetails.ProfilePhoto.OpenReadStream();
                    using var image = await Image.LoadAsync(stream);
                    var sizes = new Dictionary<string, (int width, int height)>
                                {
                                    { "thumbnail", (300, 200) },
                                };
                    foreach (var size in sizes)
                    {
                        Image resized;
                        resized = image.Clone(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Size = new Size(size.Value.width, size.Value.height),
                                Sampler = KnownResamplers.Lanczos3
                            })
                            .GaussianSharpen(0.6f)
                        );
                        var outputPath = Path.Combine(uploadsFolder, $"PatientProfilePhoto_{command.PatientDetails.PatientCode}_{size.Key}.jpg");

                        Console.WriteLine("Patient profile create line 88: ", outputPath);

                        using (resized)
                        {
                            // Save or use the resized image here
                            await resized.SaveAsync(outputPath, new JpegEncoder
                            {
                                Quality = size.Key == "thumbnail" ? 85 : 90,
                            });
                        }
                    }
                }

                PatientWithRelativesDTO result = await _mediator.Send(command, cancellationToken);

                if (result != null)
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
                        Message = "Patient profile created successfully"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Patient profile creation failed."
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPatch("UpdatePatientInfo/{id:long}")]
        [ProducesDefaultResponseType(typeof(PatientWithRelativesDTO))]
        public async Task<IActionResult> UpdatePatientProfileAsync(long id, [FromForm] EditPatientProfileDetailsCommand command, CancellationToken cancellationToken = default)
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
                        Message = "Patient details not found"
                    });
                }

                if (id != command.PatientId)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Patient Id is not correct"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = ModelState,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Validation failed. Please check the input data."
                    });
                }

                // Additional validation for IsExistingPatient and ExistingPatientId
                if (command.PatientDetails != null)
                {
                    if (command.PatientDetails.IsExistingPatient == true && 
                        (command.PatientDetails.ExistingPatientId == null || command.PatientDetails.ExistingPatientId <= 0))
                    {
                        return BadRequest(new ApiResponseResult
                        {
                            Data = null,
                            StatusCode = 400,
                            Status = "Failed",
                            Message = "Existing Patient ID is required when IsExistingPatient is true"
                        });
                    }
                }
                if (command.PatientDetails.ProfilePhoto is not null && command.PatientDetails.ProfilePhoto.Length > 0)
                {
                    var checkFileTypeMatched = CheckFileFormatMatched(command.PatientDetails.ProfilePhoto);
                    if (checkFileTypeMatched is not null)
                        return checkFileTypeMatched;

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    using var stream = command.PatientDetails.ProfilePhoto.OpenReadStream();
                    using var image = await Image.LoadAsync(stream);
                    var sizes = new Dictionary<string, (int width, int height)>
                                {
                                    { "thumbnail", (300, 200) },
                                };
                    foreach (var size in sizes)
                    {
                        Image resized;
                        resized = image.Clone(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Size = new Size(size.Value.width, size.Value.height),
                                Sampler = KnownResamplers.Lanczos3
                            })
                            .GaussianSharpen(0.6f)
                        );
                        var outputPath = Path.Combine(uploadsFolder, $"PatientProfilePhoto_{command.PatientDetails.PatientCode}_{size.Key}.jpg");

                        Console.WriteLine("Patient Profile update line 221: ",outputPath);


                        using (resized)
                        {
                            // Save or use the resized image here
                            await resized.SaveAsync(outputPath, new JpegEncoder
                            {
                                Quality = size.Key == "thumbnail" ? 85 : 90,
                            });
                        }
                    }
                }

                PatientWithRelativesDTO result = await _mediator.Send(command, cancellationToken);

                if (result != null)
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
                        Message = "Patient infomation updated"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "Patient update failed."
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        [HttpGet("GetPatientDropdown")]
        [ProducesDefaultResponseType(typeof(PatientDropdownDTO))]
        public async Task<IActionResult> GetPatientProfileDetialsAsync([FromQuery] long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetPatientDropdownQuery { UserId = userId }, cancellationToken);
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
                        Message = "Patient list found!"
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

        /// <summary>
        /// Get all patient profiles by user ID with pagination, search, and RxType filter
        /// </summary>
        /// <param name="query">Query containing user ID, RxType filter, search parameters, and paging information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of patient profiles with relatives</returns>
        [HttpPost("GetAllPatientProfilesByUserId")]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseResult), StatusCodes.Status417ExpectationFailed)]
        [ProducesDefaultResponseType(typeof(ApiResponseResult))]
        public async Task<IActionResult> GetAllPatientProfilesByUserIdAsync([FromBody] GetAllPatientProfilesByUserIdQuery query, CancellationToken cancellationToken = default)
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
                        Message = "Patient profiles found!"
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

        [HttpPost("GetPatientDetialsById")]
        [ProducesDefaultResponseType(typeof(PatientWithRelativesDTO))]
        public async Task<IActionResult> GetPatientProfileDetialsAsync([FromBody] GetPatientProfileWithRelativesByIdQuery query, CancellationToken cancellationToken = default)
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
                        Message = "Patient profile details found!"
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

      
        private ActionResult? CheckFileFormatMatched(IFormFile imageFile)
        {
           
            bool hasImage = false;
            var allowedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
           
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                
                if (allowedImageTypes.Contains(extension))
                {
                    hasImage = true;
                }
                //if (hasPdf && hasImage)
                //    break;
            
            if (!hasImage)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "File format does not match. Please select only image"
                });
            }
            return null;
        }

    }
}
