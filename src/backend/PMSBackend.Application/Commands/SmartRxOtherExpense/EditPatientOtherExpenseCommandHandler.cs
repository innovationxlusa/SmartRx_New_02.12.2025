using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Commands.SmartRxOtherExpense
{
    public class EditPatientOtherExpenseCommandHandler : IRequestHandler<EditPatientOtherExpenseCommand, SmartRxOtherExpenseDTO>
    {
        private readonly ISmartRxOtherExpenseRepository _smartRxOtherExpenseRepository;

        public EditPatientOtherExpenseCommandHandler(ISmartRxOtherExpenseRepository smartRxOtherExpenseRepository)
        {
            _smartRxOtherExpenseRepository = smartRxOtherExpenseRepository;
        }

        public async Task<SmartRxOtherExpenseDTO> Handle(EditPatientOtherExpenseCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new SmartRxOtherExpenseDTO();

            try
            {
                var otherExpense = new PatientOtherExpenseContract
                {
                    Id = request.Id,
                    SmartRxMasterId = request.SmartRxMasterId,
                    PrescriptionId = request.PrescriptionId,
                    ExpenseName = request.ExpenseName,
                    Description = request.Description,
                    Amount = request.Amount,
                    CurrencyUnitId = request.CurrencyUnitId,
                    ExpenseDate = request.ExpenseDate,
                    ExpenseNotes = request.ExpenseNotes,
                    LoginUserId = request.LoginUserId
                };

                var result = await _smartRxOtherExpenseRepository.UpdateSmartRxOtherExpenseAsync(otherExpense, cancellationToken);

                if (result != null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = result,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = $"Patient's other expense updated successfully",
                    };
                    return responseResult;
                }
                else
                {
                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = $"Patient's other expense not updated",
                    };
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                responseResult.ApiResponseResult = new ApiResponseResult()
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = $"Failed to update patient's other expense: " + ex.Message,
                };
                return responseResult;
            }
        }
    }
}