using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.Reward;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Reward;
using PMSBackend.Domain.SharedContract;
using PMSBackend.UnitTests.Common;
using System.Collections.Generic;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class RewardControllerTests : ControllerTestBase
{
    private RewardController CreateController()
    {
        ResetMocks();
        var controller = new RewardController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateReward_ReturnsOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var controller = CreateController();
        var dto = new RewardsDTO
        {
            ApiResponseResult = new ApiResponseResult
            {
                StatusCode = StatusCodes.Status200OK
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreateRewardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new CreateRewardCommand { Title = "Test", NonCashablePoints = 10 };

        // Act
        var result = await controller.CreateRewardAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto.ApiResponseResult, okResult.Value);
    }

    [Fact]
    public async Task GetAllRewards_ReturnsNotFound_WhenMediatorReturnsEmpty()
    {
        // Arrange
        var controller = CreateController();
        var paged = new PaginatedResult<RewardDTO> { Data = new List<RewardDTO>() };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRewardsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        // Act
        var result = await controller.GetAllRewardsAsync();

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(notFound.Value);
        Assert.Equal(StatusCodes.Status404NotFound, payload.StatusCode);
    }
}

