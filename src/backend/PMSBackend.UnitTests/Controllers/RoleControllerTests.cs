using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.Role;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Role;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class RoleControllerTests : ControllerTestBase
{
    private RoleController CreateController()
    {
        ResetMocks();
        var controller = new RoleController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task CreateRole_ReturnsOk_WithMediatorResult()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<RoleCreateCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        // Act
        var result = await controller.CreateRoleAsync(new RoleCreateCommand());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(5, okResult.Value);
    }

    [Fact]
    public async Task GetAllRoles_ReturnsOk_WithMediatorResult()
    {
        // Arrange
        var controller = CreateController();
        var roles = new List<RoleResponseDTO> { new() { Id = 1, RoleName = "Admin" } };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoleQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);

        // Act
        var result = await controller.GetRoleAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(roles, okResult.Value);
    }

    [Fact]
    public async Task GetRoleById_ReturnsOk_WithMediatorResult()
    {
        // Arrange
        var controller = CreateController();
        var role = new RoleResponseDTO { Id = 2 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await controller.GetRoleByIdAsync(2);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(role, okResult.Value);
    }

    [Fact]
    public async Task GetRoleByRoleName_ReturnsOk_WithMediatorResult()
    {
        // Arrange
        var controller = CreateController();
        var role = new RoleResponseDTO { Id = 3 };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoleByRoleNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await controller.GetRoleByRoleNameAsync("Manager");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(role, okResult.Value);
    }

    [Fact]
    public async Task DeleteRole_ReturnsOk_WithMediatorResult()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRoleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeleteRoleAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value!);
    }

    [Fact]
    public async Task EditRole_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var controller = CreateController();
        var command = new UpdateRoleCommand { Id = 4 };

        // Act
        var result = await controller.EditRole(5, command);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        MediatorMock.Verify(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task EditRole_ReturnsOk_WhenMediatorReturnsValue()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(7);

        var command = new UpdateRoleCommand { Id = 7 };

        // Act
        var result = await controller.EditRole(7, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(7, okResult.Value);
    }
}

