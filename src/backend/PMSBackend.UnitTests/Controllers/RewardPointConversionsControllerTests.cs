using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.RewardPointConversions;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.RewardPointConversions;
using PMSBackend.UnitTests.Common;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class RewardPointConversionsControllerTests : ControllerTestBase
{
    private RewardPointConversionsController CreateController()
    {
        ResetMocks();
        var controller = new RewardPointConversionsController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateRewardPointConversion_ReturnsOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var controller = CreateController();
        var response = new ApiResponseResult
        {
            StatusCode = StatusCodes.Status200OK,
            Status = "Success"
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreateRewardPointConversionsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var command = new CreateRewardPointConversionsCommand
        {
            dto = new RewardPointConversionsCreateDTO { UserId = 1, FromType =RewardType.Cashable, ToType = RewardType.Money, Amount = 10 }
        };

        // Act
        var result = await controller.CreateRewardPointConversionAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task GetRewardPointConversions_ReturnsOk_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var response = new ApiResponseResult { StatusCode = StatusCodes.Status200OK };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRewardPointConversionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetAllRewardPointConversionsAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }
}

