using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PMSBackend.Application.CommonServices;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.CommonServices.Validation
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates model state and returns appropriate error response
        /// </summary>
        /// <param name="modelState">The model state to validate</param>
        /// <returns>BadRequest result if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateModelState(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? new string[0]
                    );

                return new BadRequestObjectResult(new ApiResponseResult
                {
                    Data = errors,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Validation failed",
                    Errors = errors
                });
            }
            return null;
        }

        /// <summary>
        /// Validates if the request object is null
        /// </summary>
        /// <param name="request">The request object to validate</param>
        /// <param name="objectName">Name of the object for error message</param>
        /// <returns>BadRequest result if null, null if valid</returns>
        public static IActionResult? ValidateNullRequest(object? request, string objectName = "Request body")
        {
            if (request == null)
            {
                return new BadRequestObjectResult(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = $"{objectName} is required"
                });
            }
            return null;
        }

        /// <summary>
        /// Validates if an ID is positive
        /// </summary>
        /// <param name="id">The ID to validate</param>
        /// <param name="idName">Name of the ID for error message</param>
        /// <returns>BadRequest result if invalid, null if valid</returns>
        public static IActionResult? ValidatePositiveId(long id, string idName = "ID")
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = $"Valid {idName} is required"
                });
            }
            return null;
        }

        /// <summary>
        /// Validates if a string is not null or empty
        /// </summary>
        /// <param name="value">The string to validate</param>
        /// <param name="fieldName">Name of the field for error message</param>
        /// <returns>BadRequest result if invalid, null if valid</returns>
        public static IActionResult? ValidateRequiredString(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new BadRequestObjectResult(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = $"{fieldName} is required"
                });
            }
            return null;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="email">The email to validate</param>
        /// <returns>BadRequest result if invalid, null if valid</returns>
        public static IActionResult? ValidateEmail(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(email))
                {
                    return new BadRequestObjectResult(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid email format"
                    });
                }
            }
            return null;
        }

        /// <summary>
        /// Validates phone number format
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate</param>
        /// <returns>BadRequest result if invalid, null if valid</returns>
        public static IActionResult? ValidatePhoneNumber(string? phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                // Basic phone number validation - can be enhanced based on requirements
                if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[\+]?[1-9][\d]{0,15}$"))
                {
                    return new BadRequestObjectResult(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Invalid phone number format"
                    });
                }
            }
            return null;
        }

        /// <summary>
        /// Validates file upload
        /// </summary>
        /// <param name="file">The file to validate</param>
        /// <param name="maxSizeInMB">Maximum file size in MB</param>
        /// <param name="allowedExtensions">Allowed file extensions</param>
        /// <returns>BadRequest result if invalid, null if valid</returns>
        public static IActionResult? ValidateFileUpload(IFormFile? file, int maxSizeInMB = 5, string[]? allowedExtensions = null)
        {
            if (file == null) return null;

            // Check file size
            if (file.Length > maxSizeInMB * 1024 * 1024)
            {
                return new BadRequestObjectResult(new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = $"File size cannot exceed {maxSizeInMB}MB"
                });
            }

            // Check file extension
            if (allowedExtensions != null && allowedExtensions.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return new BadRequestObjectResult(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = $"File type not allowed. Allowed types: {string.Join(", ", allowedExtensions)}"
                    });
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Error message</param>
        /// <param name="exception">Exception for stack trace in development</param>
        /// <returns>ObjectResult with error response</returns>
        public static ObjectResult CreateErrorResponse(int statusCode, string message, Exception? exception = null)
        {
            return new ObjectResult(new ApiResponseResult
            {
                Data = null,
                StatusCode = statusCode,
                Status = "Failed",
                Message = message,
                StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? exception?.StackTrace : null
            })
            {
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a standardized success response
        /// </summary>
        /// <param name="data">Response data</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Success message</param>
        /// <returns>ObjectResult with success response</returns>
        public static ObjectResult CreateSuccessResponse(object? data, int statusCode = StatusCodes.Status200OK, string message = "Operation completed successfully")
        {
            return new ObjectResult(new ApiResponseResult
            {
                Data = data,
                StatusCode = statusCode,
                Status = "Success",
                Message = message
            })
            {
                StatusCode = statusCode
            };
        }
    }
}
