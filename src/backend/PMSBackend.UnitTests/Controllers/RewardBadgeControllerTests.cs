using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.RewardBadge;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.UnitTests.Common;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class RewardBadgeControllerTests : ControllerTestBase
{
    private RewardBadgeController CreateController()
    {
        ResetMocks();
        var controller = new RewardBadgeController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateRewardBadge_ReturnsOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var controller = CreateController();
        var dto = new RewardBadgesDTO
        {
            ApiResponseResult = new ApiResponseResult
            {
                StatusCode = StatusCodes.Status200OK
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreateRewardBadgeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new CreateRewardBadgeCommand { Name = "Gold", Heirarchy = 1 };

        // Act
        var result = await controller.CreateRewardBadgeAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto.ApiResponseResult, okResult.Value);
    }
}

