using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxOtherExpenseDTO
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public string ExpenseName { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public long? CurrencyUnitId { get; set; }
        public string? CurrencyUnitName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? ExpenseNotes { get; set; }
        public long LoginUserId { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}