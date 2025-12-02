using MediatR;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Queries.SmartRxOtherExpense
{
    public class GetSmartRxOtherExpensesQuery : IRequest<SmartRxOtherExpenseListDTO>
    {
        public long? SmartRxMasterId { get; set; }
        public long? PatientId { get; set; }
        public long? PrescriptionId { get; set; }
    }
}
