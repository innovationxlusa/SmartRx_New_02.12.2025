using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.BrowseRx;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.SharedContract;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class BrowseRxControllerTests : ControllerTestBase
{
    private BrowseRxController CreateController()
    {
        ResetMocks();
        var controller = new BrowseRxController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetBrowseRxFiles_ReturnsSuccess_WhenMediatorReturnsFolderTree()
    {
        // Arrange
        var controller = CreateController();
        var query = new GetBrowseRxQuery();
        var folderTree = new FolderNodeDTO
        {
            Children = new PaginatedResult<FolderNodeDTO>
            {
                Data = new List<FolderNodeDTO>
                {
                    new() { IsFolder = true, CreatedDate = DateTime.UtcNow },
                    new() { IsFolder = false, CreatedDate = DateTime.UtcNow.AddMinutes(-5) }
                }
            }
        };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetBrowseRxQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(folderTree);

        // Act
        var result = await controller.GetBrwoseRxFilesAndFoldersListAsync(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(folderTree, payload.Data);
    }

    [Fact]
    public async Task GetBrowseRxFiles_ReturnsExpectationFailed_WhenMediatorReturnsNull()
    {
        // Arrange
        var controller = CreateController();
        var query = new GetBrowseRxQuery();

        // Act
        var result = await controller.GetBrwoseRxFilesAndFoldersListAsync(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<GetBrowseRxQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetPatientPrescriptions_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var query = new GetPatientPrescriptionsQuery();
        var resultData = new PaginatedResult<PatientPrescriptionDTO>();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetPatientPrescriptionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultData);

        // Act
        var result = await controller.GetPatientPrescriptionsAsync(query);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(resultData, payload.Data);
    }

    [Fact]
    public async Task GetPatientPrescriptionsByType_ReturnsSuccess_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var request = new PatientPrescriptionByTypeRequestDTO
        {
            UserId = 1,
            PatientId = 2,
            PrescriptionType = "smartrx",
            PagingSorting = new PagingSortingParams()
        };

        var resultData = new PaginatedResult<PrescriptionDTO>();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetPatientPrescriptionsByTypeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultData);

        // Act
        var result = await controller.GetPatientPrescriptionsByTypeAsync(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(resultData, payload.Data);
    }
}

