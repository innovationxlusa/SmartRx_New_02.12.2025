using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Vital;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class VitalControllerTests : ControllerTestBase
{
    private VitalController CreateController()
    {
        ResetMocks();
        var controller = new VitalController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetAllFolders_ReturnsNotFound_WhenVitalNameMissing()
    {
        // Arrange
        var controller = CreateController();
        var query = new GetAllVitalByVitalNameQuery { VitalName = string.Empty };

        // Act
        var result = await controller.GetAllFolders(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<GetAllVitalByVitalNameQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllFolders_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var vitals = new List<VitalDTO>
        {
            new() { Id = 1, Name = "Pulse" }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllVitalByVitalNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vitals);

        var query = new GetAllVitalByVitalNameQuery { VitalName = "Pulse" };

        // Act
        var result = await controller.GetAllFolders(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(vitals, payload.Data);
    }
}

