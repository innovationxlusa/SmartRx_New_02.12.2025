using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.PatientReward;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientReward;
using PMSBackend.Domain.SharedContract;
using PMSBackend.UnitTests.Common;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class PatientRewardControllerTests : ControllerTestBase
{
    private PatientRewardController CreateController()
    {
        ResetMocks();
        var controller = new PatientRewardController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreatePatientReward_ReturnsOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var controller = CreateController();
        var dto = new PatientRewardsDTO
        {
            ApiResponseResult = new ApiResponseResult
            {
                StatusCode = StatusCodes.Status200OK
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreatePatientRewardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new CreatePatientRewardCommand
        {
            PatientId = 1,
            BadgeId = 2
        };

        // Act
        var result = await controller.CreatePatientRewardAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto.ApiResponseResult, okResult.Value);
    }

    [Fact]
    public async Task GetPatientRewardsSummary_ReturnsNotFound_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetPatientRewardsSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApiResponseResult?)null);

        // Act
        var result = await controller.GetPatientRewardsSummaryAsync(1, null);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(notFound.Value);
        Assert.Equal(StatusCodes.Status404NotFound, payload.StatusCode);
    }
}

