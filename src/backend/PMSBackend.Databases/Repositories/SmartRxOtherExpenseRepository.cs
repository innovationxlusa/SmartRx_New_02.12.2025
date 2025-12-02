using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class SmartRxOtherExpenseRepository : ISmartRxOtherExpenseRepository
    {
        private readonly PMSDbContext _dbContext;

        public SmartRxOtherExpenseRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PatientOtherExpenseContract>> GetSmartRxOtherExpensesAsync(
            long? id = null,
            long? smartRxMasterId = null,
            long? patientId = null,
            long? prescriptionId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.SmartRx_PatientOtherExpenses.AsQueryable();

                // Apply filters based on provided parameters
                if (id.HasValue)
                {
                    query = query.Where(oe => oe.Id == id.Value);
                }

                if (smartRxMasterId.HasValue)
                {
                    query = query.Where(oe => oe.SmartRxMasterId == smartRxMasterId.Value);
                }

                if (patientId.HasValue)
                {
                    query = query.Where(oe => oe.SmartRxMaster.PatientId == patientId.Value);
                }

                if (prescriptionId.HasValue)
                {
                    query = query.Where(oe => oe.PrescriptionId == prescriptionId.Value);
                }

                var result = await query
                    .Select(oe => new PatientOtherExpenseContract
                    {
                        Id = oe.Id,
                        SmartRxMasterId = oe.SmartRxMasterId,
                        PrescriptionId = oe.PrescriptionId,
                        ExpenseName = oe.ExpenseName,
                        Description = oe.Description,
                        Amount = oe.Amount,
                        CurrencyUnitId = oe.CurrencyUnitId,
                        CurrencyUnitName = oe.CurrencyUnit != null ? oe.CurrencyUnit.Name : null,
                        ExpenseDate = oe.ExpenseDate,
                        ExpenseNotes = oe.ExpenseNotes,
                        LoginUserId = oe.CreatedById ?? 0
                    })
                    .ToListAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get SmartRx other expenses: " + ex.Message);
            }
        }

        public async Task<PatientOtherExpenseContract> CreateSmartRxOtherExpenseAsync(
            PatientOtherExpenseContract otherExpense,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = new SmartRx_PatientOtherExpenseEntity
                {
                    SmartRxMasterId = otherExpense.SmartRxMasterId,
                    PrescriptionId = otherExpense.PrescriptionId,
                    ExpenseName = otherExpense.ExpenseName,
                    Description = otherExpense.Description,
                    Amount = otherExpense.Amount,
                    CurrencyUnitId = otherExpense.CurrencyUnitId,
                    ExpenseDate = otherExpense.ExpenseDate,
                    ExpenseNotes = otherExpense.ExpenseNotes,
                    CreatedDate = DateTime.Now,
                    CreatedById = otherExpense.LoginUserId
                };

                _dbContext.SmartRx_PatientOtherExpenses.Add(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var result = new PatientOtherExpenseContract
                {
                    Id = entity.Id,
                    SmartRxMasterId = entity.SmartRxMasterId,
                    PrescriptionId = entity.PrescriptionId,
                    ExpenseName = entity.ExpenseName,
                    Description = entity.Description,
                    Amount = entity.Amount,
                    CurrencyUnitId = entity.CurrencyUnitId,
                    ExpenseDate = entity.ExpenseDate,
                    ExpenseNotes = entity.ExpenseNotes,
                    LoginUserId = entity.CreatedById ?? 0
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create SmartRx other expense: " + ex.Message);
            }
        }

        public async Task<PatientOtherExpenseContract> UpdateSmartRxOtherExpenseAsync(
            PatientOtherExpenseContract otherExpense,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _dbContext.SmartRx_PatientOtherExpenses
                    .FirstOrDefaultAsync(oe => oe.Id == otherExpense.Id, cancellationToken);

                if (entity == null)
                {
                    throw new Exception("SmartRx other expense not found");
                }

                entity.ExpenseName = otherExpense.ExpenseName;
                entity.Description = otherExpense.Description;
                entity.Amount = otherExpense.Amount;
                entity.CurrencyUnitId = otherExpense.CurrencyUnitId;
                entity.ExpenseDate = otherExpense.ExpenseDate;
                entity.ExpenseNotes = otherExpense.ExpenseNotes;
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedById = otherExpense.LoginUserId;

                await _dbContext.SaveChangesAsync(cancellationToken);

                var result = new PatientOtherExpenseContract
                {
                    Id = entity.Id,
                    SmartRxMasterId = entity.SmartRxMasterId,
                    PrescriptionId = entity.PrescriptionId,
                    ExpenseName = entity.ExpenseName,
                    Description = entity.Description,
                    Amount = entity.Amount,
                    CurrencyUnitId = entity.CurrencyUnitId,
                    ExpenseDate = entity.ExpenseDate,
                    ExpenseNotes = entity.ExpenseNotes,
                    LoginUserId = entity.CreatedById ?? 0
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update SmartRx other expense: " + ex.Message);
            }
        }

        public async Task<bool> DeleteSmartRxOtherExpenseAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _dbContext.SmartRx_PatientOtherExpenses
                    .FirstOrDefaultAsync(oe => oe.Id == id, cancellationToken);

                if (entity == null)
                {
                    return false;
                }

                _dbContext.SmartRx_PatientOtherExpenses.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete SmartRx other expense: " + ex.Message);
            }
        }
    }
}