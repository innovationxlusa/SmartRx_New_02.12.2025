

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PMSBackend.Application.CommonServices.Validation
{
    public static class ControllerValidationExtensions
    {
        /// <summary>
        /// Validates request and model state for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="request">The request object to validate</param>
        /// <param name="objectName">Name of the object for error messages</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateRequest<T>(this ControllerBase controller, T? request, string objectName = "Request body") where T : class
        {
            // Null check
            var nullCheckResult = ValidationHelper.ValidateNullRequest(request, objectName);
            if (nullCheckResult != null) return nullCheckResult;

            // Model state validation
            var modelStateResult = ValidationHelper.ValidateModelState(controller.ModelState);
            if (modelStateResult != null) return modelStateResult;

            return null;
        }

        /// <summary>
        /// Validates ID parameters for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="id">The ID to validate</param>
        /// <param name="idName">Name of the ID for error messages</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateId(this ControllerBase controller, long id, string idName = "ID")
        {
            return ValidationHelper.ValidatePositiveId(id, idName);
        }

        /// <summary>
        /// Validates string parameters for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="value">The string to validate</param>
        /// <param name="fieldName">Name of the field for error messages</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateString(this ControllerBase controller, string? value, string fieldName)
        {
            return ValidationHelper.ValidateRequiredString(value, fieldName);
        }

        /// <summary>
        /// Validates email parameters for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="email">The email to validate</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateEmail(this ControllerBase controller, string? email)
        {
            return ValidationHelper.ValidateEmail(email);
        }

        /// <summary>
        /// Validates phone number parameters for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="phoneNumber">The phone number to validate</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidatePhoneNumber(this ControllerBase controller, string? phoneNumber)
        {
            return ValidationHelper.ValidatePhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Validates file upload parameters for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="file">The file to validate</param>
        /// <param name="maxSizeInMB">Maximum file size in MB</param>
        /// <param name="allowedExtensions">Allowed file extensions</param>
        /// <returns>IActionResult if validation fails, null if validation passes</returns>
        public static IActionResult? ValidateFile(this ControllerBase controller, IFormFile? file, int maxSizeInMB = 5, string[]? allowedExtensions = null)
        {
            return ValidationHelper.ValidateFileUpload(file, maxSizeInMB, allowedExtensions);
        }

        /// <summary>
        /// Creates a standardized error response for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Error message</param>
        /// <param name="exception">Exception for stack trace in development</param>
        /// <returns>ObjectResult with error response</returns>
        public static ObjectResult CreateErrorResponse(this ControllerBase controller, int statusCode, string message, Exception? exception = null)
        {
            return ValidationHelper.CreateErrorResponse(statusCode, message, exception);
        }

        /// <summary>
        /// Creates a standardized success response for controller actions
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="data">Response data</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Success message</param>
        /// <returns>ObjectResult with success response</returns>
        public static ObjectResult CreateSuccessResponse(this ControllerBase controller, object? data, int statusCode = StatusCodes.Status200OK, string message = "Operation completed successfully")
        {
            return ValidationHelper.CreateSuccessResponse(data, statusCode, message);
        }
    }
}
