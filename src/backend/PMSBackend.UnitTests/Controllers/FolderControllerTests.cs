using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.Folders;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PatientFolders;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class FolderControllerTests : ControllerTestBase
{
    private FolderController CreateController()
    {
        ResetMocks();
        var controller = new FolderController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateFolder_ReturnsCreated_WhenMediatorReturnsFolder()
    {
        // Arrange
        var controller = CreateController();
        var dto = new UserWiseFolderDTO { Id = 1 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<CreateFolderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.CreateFolder(new CreateFolderCommand());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(dto, payload.Data);
    }

    [Fact]
    public async Task UpdateFolder_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var controller = CreateController();
        var command = new UpdateFolderCommand { Id = 2 };

        // Act
        var result = await controller.FolderUpdateAsync(1, command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        MediatorMock.Verify(m => m.Send(It.IsAny<UpdateFolderCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateFolder_ReturnsOk_WhenMediatorReturnsFolder()
    {
        // Arrange
        var controller = CreateController();
        var dto = new UserWiseFolderDTO { Id = 2 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateFolderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new UpdateFolderCommand { Id = 5 };

        // Act
        var result = await controller.FolderUpdateAsync(5, command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(dto, payload.Data);
    }

    [Fact]
    public async Task DeleteFolder_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var controller = CreateController();
        var command = new DeleteFolderCommand { FolderId = 3 };

        // Act
        var result = await controller.DeleteUser(1, command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        MediatorMock.Verify(m => m.Send(It.IsAny<DeleteFolderCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteFolder_ReturnsOk_WhenMediatorConfirmsDeletion()
    {
        // Arrange
        var controller = CreateController();
        var dto = new UserWiseFolderDTO { FolderName = "Test", IsDeleted = true };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteFolderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var command = new DeleteFolderCommand { FolderId = 3 };

        // Act
        var result = await controller.DeleteUser(3, command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetAllFolders_ReturnsOk_WhenMediatorReturnsData()
    {
        // Arrange
        var controller = CreateController();
        var folders = new List<UserWiseFolderDTO> { new() { Id = 1 } };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllFolderListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(folders);

        // Act
        var result = await controller.GetAllFolders(5);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(folders, payload.Data);
    }
}

