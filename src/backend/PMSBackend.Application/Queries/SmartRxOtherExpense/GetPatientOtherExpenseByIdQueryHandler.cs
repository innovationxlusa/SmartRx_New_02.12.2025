using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.SmartRxOtherExpense
{
    public class GetPatientOtherExpenseByIdQueryHandler : IRequestHandler<GetPatientOtherExpenseByIdQuery, SmartRxOtherExpenseDTO>
    {
        private readonly ISmartRxOtherExpenseRepository _smartRxOtherExpenseRepository;

        public GetPatientOtherExpenseByIdQueryHandler(ISmartRxOtherExpenseRepository smartRxOtherExpenseRepository)
        {
            _smartRxOtherExpenseRepository = smartRxOtherExpenseRepository;
        }

        public async Task<SmartRxOtherExpenseDTO> Handle(GetPatientOtherExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new SmartRxOtherExpenseDTO();

            try
            {
                var result = await _smartRxOtherExpenseRepository.GetSmartRxOtherExpensesAsync(
                    request.Id,
                    null,
                    null,
                    null,
                    cancellationToken);

                if (result != null && result.Any())
                {
                    var expense = result.First();

                    // Map the contract to DTO
                    responseResult.Id = expense.Id;
                    responseResult.SmartRxMasterId = expense.SmartRxMasterId;
                    responseResult.PrescriptionId = expense.PrescriptionId;
                    responseResult.ExpenseName = expense.ExpenseName;
                    responseResult.Description = expense.Description;
                    responseResult.Amount = expense.Amount;
                    responseResult.CurrencyUnitId = expense.CurrencyUnitId;
                    responseResult.CurrencyUnitName = expense.CurrencyUnitName;
                    responseResult.ExpenseDate = expense.ExpenseDate;
                    responseResult.ExpenseNotes = expense.ExpenseNotes;
                    responseResult.LoginUserId = expense.LoginUserId;

                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = expense,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Patient other expense retrieved successfully",
                    };
                }
                else
                {
                    responseResult.ApiResponseResult = new ApiResponseResult()
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Patient other expense not found",
                    };
                }

                return responseResult;
            }
            catch (Exception ex)
            {
                responseResult.ApiResponseResult = new ApiResponseResult()
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Failed to get patient other expense: " + ex.Message,
                };
                return responseResult;
            }
        }
    }
}