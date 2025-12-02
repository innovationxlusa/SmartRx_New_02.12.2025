using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.DoctorProfile;
using PMSBackend.Domain.SharedContract;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class DoctorControllerTests : ControllerTestBase
{
    private DoctorController CreateController()
    {
        ResetMocks();
        var controller = new DoctorController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetPatientProfileDetails_ReturnsSuccess_WhenMediatorReturnsDoctor()
    {
        // Arrange
        var controller = CreateController();
        var dto = new DoctorProfileDTO { DoctorId = 10, ApiResponseResult = null! };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDoctorProfileByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.GetPatientProfileDetialsAsync(new GetDoctorProfileByIdQuery());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(dto, payload.Data);
    }

    [Fact]
    public async Task GetDoctorProfilesByUserId_ReturnsSuccess_WhenMediatorReturnsPage()
    {
        // Arrange
        var controller = CreateController();
        var page = new PaginatedResult<DoctorProfileListItemDTO>();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDoctorProfilesByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        // Act
        var result = await controller.GetDoctorProfilesByUserIdAsync(new GetDoctorProfilesByUserIdQuery());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(page, payload.Data);
    }
}

