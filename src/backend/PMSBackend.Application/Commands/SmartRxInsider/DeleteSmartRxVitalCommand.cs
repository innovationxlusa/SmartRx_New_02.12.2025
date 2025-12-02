using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class DeleteSmartRxVitalCommand : IRequest<DeleteDTO>
    {
        public long SmartRxVitalId { get; set; }

    }

    public class DeleteSmartRxVitalCommandHandler : IRequestHandler<DeleteSmartRxVitalCommand, DeleteDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxVitalRepository _smartRxVitalRepository;
        public DeleteSmartRxVitalCommandHandler(ISmartRxVitalRepository smartRxVitalRepository)
        {
            _smartRxVitalRepository = smartRxVitalRepository;
        }

        public async Task<DeleteDTO> Handle(DeleteSmartRxVitalCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                DeleteDTO responseResult = new DeleteDTO();

                var smartRxVital = await _smartRxVitalRepository.GetDetailsByIdAsync(request.SmartRxVitalId);

                if (smartRxVital is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Vital not found",
                    };
                    return responseResult;
                }
                var result = await _smartRxVitalRepository.DeleteAsync(smartRxVital.Id);
                if (result)
                {
                    responseResult.IsDeleted = result;
                    responseResult.ApiResponseResult = null;
                }
                await Task.CompletedTask;
                return responseResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}