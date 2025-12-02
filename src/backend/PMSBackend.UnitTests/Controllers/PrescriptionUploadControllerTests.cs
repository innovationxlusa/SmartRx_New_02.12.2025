using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.Commands.PrescriptionUpload;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.PrescriptionUpload;
using PMSBackend.UnitTests.Common;
using System.Collections.Generic;
using System.Threading;

namespace PMSBackend.UnitTests.Controllers;

public class PrescriptionUploadControllerTests : ControllerTestBase
{
    private PrescriptionUploadController CreateController()
    {
        ResetMocks();
        var controller = new PrescriptionUploadController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task FileUpload_ReturnsNotFound_WhenFilesAreMissing()
    {
        // Arrange
        var controller = CreateController();
        var command = new InsertPrescriptionUploadCommand();

        // Act
        var result = await controller.FileUploadAsync(new List<IFormFile>(), command);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        MediatorMock.Verify(m => m.Send(It.IsAny<InsertPrescriptionUploadCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeletePrescription_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var controller = CreateController();
        var command = new DeletePrescriptionCommand { PrescriptionId = 2 };

        // Act
        var result = await controller.DeletePrescription(1, command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        MediatorMock.Verify(m => m.Send(It.IsAny<DeletePrescriptionCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

