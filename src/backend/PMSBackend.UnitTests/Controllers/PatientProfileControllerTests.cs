using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.PatientProfile;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientProfile;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class PatientProfileControllerTests : ControllerTestBase
{
    private PatientProfileController CreateController()
    {
        ResetMocks();
        var controller = new PatientProfileController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreatePatientProfile_ReturnsBadRequest_WhenCommandIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.CreatePatientProfileAsync(null!);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, payload.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<CreatePatientProfileCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetPatientDropdown_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        const long userId = 42;
        var dto = new PatientDropdownDTO();
        MediatorMock
            .Setup(m => m.Send(It.Is<GetPatientDropdownQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.GetPatientProfileDetialsAsync(userId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(dto, payload.Data);
    }
}

