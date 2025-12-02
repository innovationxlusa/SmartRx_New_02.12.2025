using MediatR;
using PMSBackend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.SmartRxOtherExpense
{
    public class AddSmartRxOtherExpenseCommand : IRequest<SmartRxOtherExpenseDTO>
    {
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