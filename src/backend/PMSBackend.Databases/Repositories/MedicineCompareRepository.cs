using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class MedicineCompareRepository : IMedicineCompareRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IBaseRepository<Configuration_VitalEntity> _vitalRepository;
        private readonly IBaseRepository<SmartRx_PatientVitalsEntity> _smartRxVitalRepository;

        public MedicineCompareRepository(PMSDbContext context)
        {
            this._dbContext = context;
            _vitalRepository = new BaseRepository<Configuration_VitalEntity>(_dbContext);
            _smartRxVitalRepository = new BaseRepository<SmartRx_PatientVitalsEntity>(_dbContext);
        }


        public async Task<Configuration_MedicineEntity> CheckThisMedicineExists(long medicineId)
        {
            try
            {
                var sourceMedicine = await _dbContext.Configuration_Medicine.AsNoTracking().FirstOrDefaultAsync(m => m.Id == medicineId);

                return sourceMedicine;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<SmartRx_PatientMedicineEntity> GetMedicineWishlist(long smartRxMasterId, long PrescriptionId, long medicineId)
        {
            try
            {
                var sourceMedicine = await _dbContext.SmartRx_PatientMedicine.AsNoTracking().FirstOrDefaultAsync(m => m.Id == medicineId);
                return sourceMedicine;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SmartRx_PatientInvestigationEntity> GetInvestigationWishlist(long smartRxMasterId, long PrescriptionId, long investigatonId)
        {
            try
            {
                var sourceMedicine = await _dbContext.SmartRx_PatientInvestigation.AsNoTracking().FirstOrDefaultAsync(i => i.Id == investigatonId);
                return sourceMedicine;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PaginatedResult<MedicineInfoModel>> ListOfSameGenericOtherBrandOfAMedicine(long smartRxMasterId, long prescriptionId, long medicineId, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken)
        {
            try
            {
                if (pagingAndSorting.PageNumber <= 0) pagingAndSorting.PageNumber = 1;
                if (pagingAndSorting.PageSize <= 0) pagingAndSorting.PageSize = 10;

                var sourceMedicineWishlist = await _dbContext.SmartRx_PatientMedicine
                                         .AsNoTracking()
                                         .Where(w => w.SmartRxMasterId == smartRxMasterId && w.PrescriptionId == prescriptionId && w.MedicineId == medicineId)
                                         .Select(s => new { s.MedicineId, s.Wishlist })
                                         .ToListAsync(cancellationToken);

                // Process wishlist in memory (allowed to use TryParse here)
                var allWishedMedicineIds = sourceMedicineWishlist
                    .SelectMany(x =>
                        (x.Wishlist ?? "")
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                            .Where(tid => tid.HasValue)
                            .Select(tid => tid.Value)
                        )
                        .ToHashSet();

                var generic = await _dbContext.Configuration_Medicine
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == medicineId, cancellationToken);

                var otherBrandMedicineList = await _dbContext.Configuration_Medicine
                    .AsNoTracking()
                    .Where(m => m.GenericId == generic!.GenericId)
                    .Include(m => m.Brand)
                        .ThenInclude(b => b.Manufacturer)
                    .Include(m => m.Generic)
                    .Include(m => m.MedicineDosageForm)
                    .Include(m => m.MeasurementUnit)
                    .Include(m => m.PriceUnit)
                    .Select(m => new MedicineInfoModel
                    {
                        MedicineId = m.Id,
                        MedicineName = m.Name,
                        BrandCode = m.Brand.BrandCode,
                        BrandName = m.Brand.Name,
                        ManufacturerName = m.Brand.Manufacturer.Name,
                        ManufacturerOriginRegion = m.Brand.Manufacturer.OriginRegion,
                        Importer = m.Brand.Manufacturer.Importer,
                        Type = m.Type,
                        Slug = m.Slug,
                        MedicineDosageFormName = m.MedicineDosageForm.Name,
                        MedicineDosageFormShortForm = m.MedicineDosageForm.ShortForm,
                        GenericName = m.Generic.Name,
                        Strength = m.Strength,
                        MeasurementUnit = m.MeasurementUnit!.MeasurementUnit,
                        MeasurementUnitName = m.MeasurementUnit.Name,
                        MeasurementUnitType = m.MeasurementUnit.Type,
                        UnitPriceValue = m.UnitPrice,
                        UnitPriceName = m.PriceUnit!.Name,
                        UnitPriceType = m.PriceUnit.Type,
                        UserFor = m.UserFor,
                        CompanyDiscount = m.CompanyDiscountPercentage,
                        IsActive = m.IsActive,
                    })
                    .ToListAsync(cancellationToken);

                // Set wished flag
                foreach (var med in otherBrandMedicineList)
                {
                    med.Wished = allWishedMedicineIds.Contains(med.MedicineId);
                    med.WishList = allWishedMedicineIds.Contains(med.MedicineId) ? sourceMedicineWishlist.FirstOrDefault(w => w.MedicineId == med.MedicineId)?.Wishlist : null;
                }

                var totalRecords = otherBrandMedicineList.Count();

                IEnumerable<MedicineInfoModel> sortedQuery;
                switch (pagingAndSorting.SortBy?.ToLower())
                {
                    case "price":
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? otherBrandMedicineList.OrderByDescending(m => m.UnitPriceValue)
                            : otherBrandMedicineList.OrderBy(m => m.UnitPriceValue);
                        break;
                    case "name":
                    default:
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? otherBrandMedicineList.OrderByDescending(m => m.MedicineName)
                            : otherBrandMedicineList.OrderBy(m => m.MedicineName);
                        break;
                }

                var pagedResult = sortedQuery
                                 .Skip((pagingAndSorting.PageNumber - 1) * pagingAndSorting.PageSize)
                .Take(pagingAndSorting.PageSize).ToList();
                return new PaginatedResult<MedicineInfoModel>(pagedResult, totalRecords, pagingAndSorting.PageNumber, pagingAndSorting.PageSize, pagingAndSorting.SortBy, pagingAndSorting.SortDirection, null, null, null, null, null); ;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public Task CompareMedicine(long compareToMedicineId)
        {
            //var sourceMedicine = await _dbContext.Configuration_Medicine.FindAsync(id);
            //if (sourceMedicine == null)
            //    return NotFound("Product not found.");
            throw new NotImplementedException();
        }
    }
}
