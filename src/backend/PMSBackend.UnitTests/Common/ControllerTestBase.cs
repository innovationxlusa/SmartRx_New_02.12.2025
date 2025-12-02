using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace PMSBackend.UnitTests.Common;

public abstract class ControllerTestBase
{
    protected Mock<IMediator> MediatorMock { get; } = new();

    protected void ResetMocks()
    {
        MediatorMock.Reset();
    }

    protected T SetupController<T>(T controller) where T : ControllerBase
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        return controller;
    }

    protected static void MakeModelStateInvalid(ControllerBase controller, string key = "model", string error = "Invalid")
    {
        controller.ModelState.AddModelError(key, error);
    }
}

