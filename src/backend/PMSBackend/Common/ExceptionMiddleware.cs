using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Exceptions;
using System.Text.Json;

namespace PMSBackend.API.Common
{
    public class ExceptionMiddleware //: IMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string _LogApiUrl;

        private readonly string _AppName;

        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            //_LogApiUrl = config.APIDomain + "/v2/log";
            _AppName = "SmartRxApp";
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception occurred | Type: {ExceptionType} | Message: {Message} | Inner Exception: {InnerException} | Stack Trace: {StackTrace}",
                    ex.GetType().Name,
                    ex.Message,
                    ex.InnerException?.Message ?? "None",
                    ex.StackTrace ?? "No stack trace available");
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = DetermineStatusCode(exception);
            context.Response.StatusCode = statusCode;
            var response = BuildErrorResponse(exception, statusCode);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private int DetermineStatusCode(Exception exception)
        {
            // Handle custom exceptions with ErrorCode property (prioritize custom error codes)
            if (exception is NotFoundException notFoundEx && notFoundEx.ErrorCode > 0)
                return notFoundEx.ErrorCode;

            if (exception is BadRequestException badRequestEx && badRequestEx.ErrorCode > 0)
                return badRequestEx.ErrorCode;

            if (exception is ConflictException conflictEx && conflictEx.ErrorCode > 0)
                return conflictEx.ErrorCode;

            if (exception is UnprocessableEntityException unprocessableEx && unprocessableEx.ErrorCode > 0)
                return unprocessableEx.ErrorCode;

            if (exception is ForbiddenAccessException forbiddenEx && forbiddenEx.ErrorCode > 0)
                return forbiddenEx.ErrorCode;

            // Map exception types to HTTP status codes
            return exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnprocessableEntityException => StatusCodes.Status422UnprocessableEntity,
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ForbiddenAccessException => StatusCodes.Status403Forbidden,
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                InvalidOperationException => StatusCodes.Status409Conflict,
                TimeoutException => StatusCodes.Status408RequestTimeout,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                OperationCanceledException => StatusCodes.Status499ClientClosedRequest,
                SqlException => StatusCodes.Status500InternalServerError, // Database errors
                DbUpdateException => StatusCodes.Status500InternalServerError, // EF Core errors

                _ => StatusCodes.Status500InternalServerError // Default to 500 for unknown exceptions
            };
        }

        private ApiResponseResult BuildErrorResponse(Exception exception, int statusCode)
        {
            // Extract specific error details
            string errorMessage = ExtractErrorMessage(exception);

            var response = new ApiResponseResult
            {
                Status = "Failed",
                StatusCode = statusCode,
                Message = errorMessage,
                HRResult = exception.HResult
            };

            // Add detailed information in development environment
            if (_env.IsDevelopment())
            {
                response.StackTrace = exception.StackTrace;

                // Build detailed message with all exception info
                var detailedMessage = errorMessage;

                // Add exception type
                detailedMessage += $" [Exception Type: {exception.GetType().Name}]";

                // Add inner exception details if available
                if (exception.InnerException != null)
                {
                    detailedMessage += $" | Inner Exception: {exception.InnerException.GetType().Name} - {exception.InnerException.Message}";

                    // Add inner stack trace for more debugging
                    if (!string.IsNullOrEmpty(exception.InnerException.StackTrace))
                    {
                        detailedMessage += $" | Inner StackTrace: {exception.InnerException.StackTrace}";
                    }
                }

                // Add source if available
                if (!string.IsNullOrEmpty(exception.Source))
                {
                    detailedMessage += $" | Source: {exception.Source}";
                }

                // Add target site (method) if available
                if (exception.TargetSite != null)
                {
                    detailedMessage += $" | Method: {exception.TargetSite.Name}";
                }

                response.Message = detailedMessage;
            }
            else
            {
                // In production, provide user-friendly messages based on status code
                response.Message = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "Invalid request. Please verify your input data.",
                    StatusCodes.Status401Unauthorized => "Authentication required. Please log in.",
                    StatusCodes.Status403Forbidden => "Access forbidden. You don't have permission to access this resource.",
                    StatusCodes.Status404NotFound => "The requested resource was not found.",
                    StatusCodes.Status409Conflict => "A conflict occurred. The resource may have been modified by another user.",
                    StatusCodes.Status422UnprocessableEntity => "The request was well-formed but contains semantic errors.",
                    StatusCodes.Status408RequestTimeout => "The request timed out. Please try again.",
                    StatusCodes.Status500InternalServerError => "An internal server error occurred. Please contact the system administrator.",
                    StatusCodes.Status501NotImplemented => "This feature is not yet implemented.",
                    StatusCodes.Status499ClientClosedRequest => "The request was cancelled.",
                    _ => "An error occurred. Please contact the system administrator."
                };
            }

            // Handle ValidationException errors dictionary
            if (exception is ValidationException validationEx && validationEx.Errors != null)
            {
                response.Errors = (Dictionary<string, string[]>?)validationEx.Errors;
            }

            return response;
        }

        private string ExtractErrorMessage(Exception exception)
        {
            // Return the most specific error message
            var message = exception.Message ?? "An unknown error occurred";

            // If no meaningful message, provide a generic one
            if (string.IsNullOrWhiteSpace(message) || message.Length < 5)
            {
                message = $"Error of type {exception.GetType().Name} occurred";
            }

            return message;
        }


       
    }

}