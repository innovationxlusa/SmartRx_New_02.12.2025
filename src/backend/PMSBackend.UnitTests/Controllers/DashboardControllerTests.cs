using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Dashboard;
using PMSBackend.Controllers;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class DashboardControllerTests : ControllerTestBase
{
    private DashboardController CreateController()
    {
        ResetMocks();
        var controller = new DashboardController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetDashboardSummary_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var summary = new DashboardSummaryDTO();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDashboardSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(summary);

        // Act
        var result = await controller.GetDashboardSummary(1, null);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(summary, payload.Data);
    }

    [Fact]
    public async Task GetDashboardSummary_ReturnsServerError_WhenMediatorThrows()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDashboardSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("failure"));

        // Act
        var result = await controller.GetDashboardSummary(1, null);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetDashboardCounts_ReturnsOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var controller = CreateController();
        var dto = new DashboardCountsDTO
        {
            ApiResponseResult = new ApiResponseResult
            {
                StatusCode = StatusCodes.Status200OK
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDashboardCountsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.GetDashboardCounts(1, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task GetDashboardCounts_PropagatesStatus_WhenMediatorReturnsError()
    {
        // Arrange
        var controller = CreateController();
        var dto = new DashboardCountsDTO
        {
            ApiResponseResult = new ApiResponseResult
            {
                StatusCode = StatusCodes.Status404NotFound
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetDashboardCountsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.GetDashboardCounts(1, null);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }
}

