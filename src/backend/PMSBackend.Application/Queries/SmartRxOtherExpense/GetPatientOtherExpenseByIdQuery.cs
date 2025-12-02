using MediatR;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Queries.SmartRxOtherExpense
{
    public class GetPatientOtherExpenseByIdQuery : IRequest<SmartRxOtherExpenseDTO>
    {
        public long Id { get; set; }
    }
}
