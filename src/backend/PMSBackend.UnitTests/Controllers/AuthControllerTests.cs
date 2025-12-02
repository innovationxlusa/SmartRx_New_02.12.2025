using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.Auth;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Auth;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class AuthControllerTests : ControllerTestBase
{
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();

    private AuthController CreateController()
    {
        ResetMocks();
        _tokenGeneratorMock.Reset();

        var controller = new AuthController(MediatorMock.Object, _tokenGeneratorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task Login_ReturnsSuccess_WhenMediatorReturnsUser()
    {
        // Arrange
        var controller = CreateController();
        var loginDto = new LoginDTO { UserId = 1 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginDto);

        var command = new AuthCommand { AuthType = 0, Password = "secret" };

        // Act
        var result = await controller.Login(command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(loginDto, payload.Data);
        MediatorMock.Verify(m => m.Send(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenPasswordMissingForEmailAuth()
    {
        // Arrange
        var controller = CreateController();
        var command = new AuthCommand { AuthType = 1, Password = null };

        // Act
        var result = await controller.Login(command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal("Password is required for email authentication", payload.Message);
        MediatorMock.Verify(m => m.Send(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Login_ReturnsError_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginDTO?)null);

        var command = new AuthCommand { AuthType = 0, Password = "secret" };

        // Act
        var result = await controller.Login(command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal("User not found", payload.Message);
    }

    [Fact]
    public async Task Login_ReturnsModelValidationErrors_WhenModelStateInvalid()
    {
        // Arrange
        var controller = CreateController();
        MakeModelStateInvalid(controller);
        var command = new AuthCommand { AuthType = 0 };

        // Act
        var result = await controller.Login(command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, payload.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task VerifyOtp_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var response = new AuthResponseDTO { UserId = 5 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyOtpRequestQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var query = new VerifyOtpRequestQuery { UserId = 5, Otp = "123456" };

        // Act
        var result = await controller.VerifyOtp(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(response, payload.Data);
    }

    [Fact]
    public async Task VerifyOtp_ReturnsBadRequest_WhenUserIdInvalid()
    {
        // Arrange
        var controller = CreateController();
        var query = new VerifyOtpRequestQuery { UserId = 0, Otp = "123456" };

        // Act
        var result = await controller.VerifyOtp(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<VerifyOtpRequestQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task VerifyOtp_ReturnsExpectationFailed_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyOtpRequestQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuthResponseDTO?)null);

        var query = new VerifyOtpRequestQuery { UserId = 7, Otp = "789456" };

        // Act
        var result = await controller.VerifyOtp(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_ReturnsOk_WhenMediatorReturnsToken()
    {
        // Arrange
        var controller = CreateController();
        var token = new AuthTokenResponseDTO { AccessToken = "access", RefreshToken = "refresh" };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var command = new RefreshTokenCommand { RefreshToken = "refresh" };

        // Act
        var result = await controller.RefreshToken(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(okResult.Value);
        Assert.Equal(token, payload.Data);
    }

    [Fact]
    public async Task RefreshToken_ReturnsUnauthorized_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuthTokenResponseDTO?)null);

        var command = new RefreshTokenCommand { RefreshToken = "expired" };

        // Act
        var result = await controller.RefreshToken(command);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(unauthorizedResult.Value);
        Assert.Equal(StatusCodes.Status401Unauthorized, payload.StatusCode);
    }

    [Fact]
    public async Task RevokeToken_ReturnsOk_WhenMediatorReturnsTrue()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RevokeTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new RevokeTokenCommand { RefreshToken = "token" };

        // Act
        var result = await controller.RevokeToken(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(okResult.Value);
        Assert.True((bool?)payload.Data);
    }

    [Fact]
    public async Task RevokeToken_ReturnsNotFound_WhenMediatorReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RevokeTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new RevokeTokenCommand { RefreshToken = "missing" };

        // Act
        var result = await controller.RevokeToken(command);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(notFoundResult.Value);
        Assert.False((bool?)payload.Data);
    }

    [Fact]
    public async Task RevokeAllUserTokens_ReturnsOk_WhenMediatorReturnsTrue()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RevokeAllUserTokensCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new RevokeAllUserTokensCommand { UserId = 10 };

        // Act
        var result = await controller.RevokeAllUserTokens(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(okResult.Value);
        Assert.True((bool?)payload.Data);
    }

    [Fact]
    public async Task RevokeAllUserTokens_ReturnsNotFound_WhenMediatorReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RevokeAllUserTokensCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new RevokeAllUserTokensCommand { UserId = 11 };

        // Act
        var result = await controller.RevokeAllUserTokens(command);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(notFoundResult.Value);
        Assert.False((bool?)payload.Data);
    }

    [Fact]
    public async Task GenerateToken_ReturnsOk_WhenTokenGenerated()
    {
        // Arrange
        var controller = CreateController();
        var tokenResponse = new AuthTokenResponseDTO { AccessToken = "token" };
        _tokenGeneratorMock
            .Setup(t => t.GenerateTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenResponse);

        var request = new GenerateTokenRequest { UserId = 2 };

        // Act
        var result = await controller.GenerateToken(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(okResult.Value);
        Assert.Equal(tokenResponse, payload.Data);
    }

    [Fact]
    public async Task GenerateToken_ReturnsBadRequest_WhenUserIdInvalid()
    {
        // Arrange
        var controller = CreateController();
        var request = new GenerateTokenRequest { UserId = 0 };

        // Act
        var result = await controller.GenerateToken(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, payload.StatusCode);
        _tokenGeneratorMock.Verify(t => t.GenerateTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GenerateToken_ReturnsBadRequest_WhenGeneratorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        _tokenGeneratorMock
            .Setup(t => t.GenerateTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuthTokenResponseDTO?)null);

        var request = new GenerateTokenRequest { UserId = 3 };

        // Act
        var result = await controller.GenerateToken(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, payload.StatusCode);
    }
}

