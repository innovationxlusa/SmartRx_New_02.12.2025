using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.User;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.User;
using PMSBackend.UnitTests.Common;
using System;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class UserControllerTests : ControllerTestBase
{
    private UserController CreateController()
    {
        ResetMocks();
        var controller = new UserController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreated_WhenMediatorReturnsUser()
    {
        // Arrange
        var controller = CreateController();
        var dto = new UserDetailsResponseDTO { Email = "test@example.com" };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            MobileNo = "+880123456789",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var result = await controller.CreateUser(command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(dto, payload.Data);
    }

    [Fact]
    public void ThrowError_ThrowsException()
    {
        // Arrange
        var controller = CreateController();

        // Act & Assert
        Assert.Throws<Exception>(() => controller.ThrowError());
    }
}

