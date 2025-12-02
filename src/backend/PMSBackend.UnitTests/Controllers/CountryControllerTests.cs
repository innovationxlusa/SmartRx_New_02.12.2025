using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMSBackend.API.Controllers;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Country;
using PMSBackend.UnitTests.Common;

namespace PMSBackend.UnitTests.Controllers;

public class CountryControllerTests : ControllerTestBase
{
    private CountryController CreateController()
    {
        ResetMocks();
        var controller = new CountryController(MediatorMock.Object);
        return SetupController(controller);
    }

    [Fact]
    public async Task GetCountries_ReturnsOk_WhenMediatorSucceeds()
    {
        // Arrange
        var controller = CreateController();
        var countries = new List<CountryDTO> { new() { Id = 1, Name = "Bangladesh" } };
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetCountriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(countries);

        // Act
        var result = await controller.GetCountries();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ApiResponseResult>(okResult.Value);
        Assert.Equal(countries, payload.Data);
        Assert.Equal(StatusCodes.Status200OK, payload.StatusCode);
    }

    [Fact]
    public async Task GetCountries_ReturnsInternalServerError_WhenMediatorThrows()
    {
        // Arrange
        var controller = CreateController();
        MediatorMock
            .Setup(m => m.Send(It.IsAny<GetCountriesQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.GetCountries();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var payload = Assert.IsType<ApiResponseResult>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, payload.StatusCode);
    }
}

