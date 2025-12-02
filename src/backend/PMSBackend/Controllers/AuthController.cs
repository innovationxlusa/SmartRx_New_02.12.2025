using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.Commands.Auth;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Exceptions;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Application.CommonServices.Validation;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Auth;
using PMSBackend.Domain.Entities;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Auth endpoints should allow anonymous access
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly PasswordHasher<SmartRxUserEntity> _passwordHasher = new();
        
        public AuthController(IMediator mediator, ITokenGenerator tokenGenerator)
        {
            _mediator = mediator;
            _tokenGenerator = tokenGenerator;
        }


        [HttpPost("Login")]
        [ProducesDefaultResponseType(typeof(LoginDTO))]
        public async Task<IActionResult> Login([FromBody] AuthCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                var validationResult = this.ValidateRequest(command);
                if (validationResult != null) return validationResult;

                // Additional business validation
                if (command.AuthType == 1 && string.IsNullOrWhiteSpace(command.Password))
                {
                    return this.CreateErrorResponse(StatusCodes.Status400BadRequest, "Password is required for email authentication");
                }

                var result = await _mediator.Send(command, cancellationToken);
                if (result != null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "User found successfully");
                }
                return this.CreateErrorResponse(StatusCodes.Status400BadRequest, "User not found");
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred during login", ex);
            }
        }

        [HttpPost("verify-otp")]
        [ProducesDefaultResponseType(typeof(AuthResponseDTO))]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestQuery command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                var validationResult = this.ValidateRequest(command);
                if (validationResult != null) return validationResult;

                // Additional business validation
                var idCheckResult = this.ValidateId(command.UserId, "User ID");
                if (idCheckResult != null) return idCheckResult;

                var result = await _mediator.Send(command, cancellationToken);

                if (result is not null)
                {
                    if (result.ApiResponseResult is not null)
                    {
                        return StatusCode(result.ApiResponseResult.StatusCode, result.ApiResponseResult);
                    }
                    return this.CreateSuccessResponse(result, StatusCodes.Status200OK, "OTP verified successfully");
                }
                else
                {
                    return this.CreateErrorResponse(StatusCodes.Status417ExpectationFailed, "Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                return this.CreateErrorResponse(StatusCodes.Status500InternalServerError, "An error occurred during OTP verification", ex);
            }

        }

        [HttpPost("refresh-token")]
        [ProducesDefaultResponseType(typeof(AuthTokenResponseDTO))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid request data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                if (result != null)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Token refreshed successfully"
                    });
                }
                else
                {
                    return Unauthorized(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Status = "Failed",
                        Message = "Invalid or expired refresh token"
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
                    Message = "An error occurred while refreshing token: " + ex.Message
                });
            }
        }

        [HttpPost("revoke-token")]
        [ProducesDefaultResponseType(typeof(bool))]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid request data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                if (result)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = true,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Token revoked successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Token not found"
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
                    Message = "An error occurred while revoking token: " + ex.Message
                });
            }
        }

        [HttpPost("revoke-all-user-tokens")]
        [ProducesDefaultResponseType(typeof(bool))]
        public async Task<IActionResult> RevokeAllUserTokens([FromBody] RevokeAllUserTokensCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request and model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid request data"
                    });
                }

                var result = await _mediator.Send(command, cancellationToken);

                if (result)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = true,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "All user tokens revoked successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponseResult
                    {
                        Data = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "User not found or no tokens to revoke"
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
                    Message = "An error occurred while revoking all user tokens: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Generate token directly for testing purposes (Development use only)
        /// Allows quick token generation for Postman testing
        /// </summary>
        [HttpPost("generate-token")]
        [ProducesDefaultResponseType(typeof(AuthTokenResponseDTO))]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate request
                if (!ModelState.IsValid || request.UserId <= 0)
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid request. UserId is required."
                    });
                }

                // Generate tokens
                var result = await _tokenGenerator.GenerateTokensAsync(request.UserId, cancellationToken);

                if (result != null)
                {
                    return Ok(new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Token generated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Failed to generate token. User may not exist or have roles assigned."
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
                    Message = "An error occurred while generating token: " + ex.Message
                });
            }
        }
    }

    /// <summary>
    /// Request model for generate token endpoint
    /// </summary>
    public class GenerateTokenRequest
    {
        public long UserId { get; set; }
    }
}
