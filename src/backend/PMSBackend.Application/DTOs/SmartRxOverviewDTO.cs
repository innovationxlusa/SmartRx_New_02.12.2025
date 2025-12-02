namespace PMSBackend.Application.DTOs
{
    public class SmartRxOverviewDTO
    {
        public decimal TotalDoctorExpense { get; set; }
        public decimal TotalMedicineConsumption { get; set; }
        public decimal TotalLabInvestigationExpense { get; set; }
        public decimal TotalTransportExpense { get; set; }
        public decimal TotalTravelTime { get; set; }
        public decimal TotalWaitingTime { get; set; }
        public decimal TotalVisitingTime { get; set; }
        public decimal TotalChamberTime { get; set; }
        public decimal TotalOtherExpense { get; set; }
        public IList<SmartRxOtherExpenseDTO>? OtherExpenses { get; set; }

    }
}