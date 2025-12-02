using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.SmartRxOtherExpense
{
    public class GetSmartRxOtherExpensesQueryHandler : IRequestHandler<GetSmartRxOtherExpensesQuery, SmartRxOtherExpenseListDTO>
    {
        private readonly ISmartRxOtherExpenseRepository _smartRxOtherExpenseRepository;

        public GetSmartRxOtherExpensesQueryHandler(ISmartRxOtherExpenseRepository smartRxOtherExpenseRepository)
        {
            _smartRxOtherExpenseRepository = smartRxOtherExpenseRepository;
        }

        public async Task<SmartRxOtherExpenseListDTO> Handle(GetSmartRxOtherExpensesQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new SmartRxOtherExpenseListDTO();

            try
            {
                var result = await _smartRxOtherExpenseRepository.GetSmartRxOtherExpensesAsync(
                    null, // No ID parameter for list query
                    request.SmartRxMasterId,
                    request.PatientId,
                    request.PrescriptionId,
                    cancellationToken);

                var mapped = result?.Select(oe => new SmartRxOtherExpenseDTO
                {
                    Id = oe.Id,
                    SmartRxMasterId = oe.SmartRxMasterId,
                    PrescriptionId = oe.PrescriptionId,
                    ExpenseName = oe.ExpenseName,
                    Description = oe.Description,
                    Amount = oe.Amount,
                    CurrencyUnitId = oe.CurrencyUnitId,
                    CurrencyUnitName = oe.CurrencyUnitName,
                    ExpenseDate = oe.ExpenseDate,
                    ExpenseNotes = oe.ExpenseNotes,
                    LoginUserId = oe.LoginUserId
                }).ToList() ?? new List<SmartRxOtherExpenseDTO>();

                responseResult.Data = mapped;
                responseResult.ApiResponseResult = new ApiResponseResult()
                {
                    Data = mapped,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "SmartRx other expenses retrieved successfully",
                };
                return responseResult;
            }
            catch (Exception ex)
            {
                var empty = new List<SmartRxOtherExpenseDTO>();
                responseResult.Data = empty;
                responseResult.ApiResponseResult = new ApiResponseResult()
                {
                    Data = empty,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Failed to get SmartRx other expenses: " + ex.Message,
                };
                return responseResult;
            }
        }
    }
}
