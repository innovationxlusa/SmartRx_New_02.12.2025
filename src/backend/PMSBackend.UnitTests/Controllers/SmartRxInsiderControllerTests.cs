using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.SmartRxInsider;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.SmartRxInsider;
using PMSBackend.UnitTests.Common;
using System;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class SmartRxInsiderControllerTests : ControllerTestBase
{
    private SmartRxInsiderController CreateController()
    {
        ResetMocks();
        var controller = new SmartRxInsiderController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetSmartRxInsiderById_ReturnsExpectationFailed_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        var query = new GetSmartRxMainInsiderQuery();

        // Act
        var result = await controller.GetsmartrxinsiderbyidAsync(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<GetSmartRxMainInsiderQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EditSmartRxVital_ReturnsBadRequest_WhenArgumentExceptionRaised()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<ChangeSmartRxDoctorReviewCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("invalid"));

        var command = new ChangeSmartRxDoctorReviewCommand();

        // Act
        var result = await controller.EditSmartRxVital(command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, payload.StatusCode);
    }
}

