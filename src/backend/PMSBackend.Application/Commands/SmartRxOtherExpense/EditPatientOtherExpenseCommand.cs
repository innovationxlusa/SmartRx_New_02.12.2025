using MediatR;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Commands.SmartRxOtherExpense
{
    public class EditPatientOtherExpenseCommand : IRequest<SmartRxOtherExpenseDTO>
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public string ExpenseName { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public long? CurrencyUnitId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? ExpenseNotes { get; set; }
        public long LoginUserId { get; set; }
    }
}
