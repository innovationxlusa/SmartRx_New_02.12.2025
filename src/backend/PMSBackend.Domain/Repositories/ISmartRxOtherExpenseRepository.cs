using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface ISmartRxOtherExpenseRepository
    {
        Task<List<PatientOtherExpenseContract>> GetSmartRxOtherExpensesAsync(
            long? id = null,
            long? smartRxMasterId = null,
            long? patientId = null,
            long? prescriptionId = null,
            CancellationToken cancellationToken = default);

        Task<PatientOtherExpenseContract> CreateSmartRxOtherExpenseAsync(
            PatientOtherExpenseContract otherExpense,
            CancellationToken cancellationToken = default);

        Task<PatientOtherExpenseContract> UpdateSmartRxOtherExpenseAsync(
            PatientOtherExpenseContract otherExpense,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteSmartRxOtherExpenseAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
