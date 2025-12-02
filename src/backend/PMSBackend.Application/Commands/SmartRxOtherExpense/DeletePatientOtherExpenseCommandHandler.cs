using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxOtherExpense
{
    public class DeletePatientOtherExpenseCommandHandler : IRequestHandler<DeletePatientOtherExpenseCommand, SmartRxOtherExpenseDTO>
    {
        private readonly ISmartRxOtherExpenseRepository _smartRxOtherExpenseRepository;

        public DeletePatientOtherExpenseCommandHandler(ISmartRxOtherExpenseRepository smartRxOtherExpenseRepository)
        {
            _smartRxOtherExpenseRepository = smartRxOtherExpenseRepository;
        }

        public async Task<SmartRxOtherExpenseDTO> Handle(DeletePatientOtherExpenseCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new SmartRxOtherExpenseDTO();

            try
            {
                var result = await _smartRxOtherExpenseRepository.DeleteSmartRxOtherExpenseAsync(request.Id, cancellationToken);

                if (result)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = true,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = $"Patient's other expense deleted successfully",
                    };
                    return responseResult;
                }
                else
                {
                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = $"Patient's other expense not found",
                    };
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.ApiResponseResult = new ApiResponseResult()
                {
                    Data = false,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = $"Failed to delete patient's other expense: " + ex.Message,
                };
                return responseResult;
            }
        }
    }
}