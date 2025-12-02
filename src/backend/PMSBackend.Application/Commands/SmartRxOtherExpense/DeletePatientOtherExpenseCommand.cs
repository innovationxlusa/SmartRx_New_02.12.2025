using MediatR;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Commands.SmartRxOtherExpense
{
    public class DeletePatientOtherExpenseCommand : IRequest<SmartRxOtherExpenseDTO>
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
    }
}