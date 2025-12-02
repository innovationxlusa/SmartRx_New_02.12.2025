using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Databases.Repositories
{
    public class DoctorProfileRepository : IDoctorProfileRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly ICodeGenerationService _codeGenerationService;

        public DoctorProfileRepository(PMSDbContext context, ICodeGenerationService codeGenerationService)
        {
            _dbContext = context;
            _codeGenerationService = codeGenerationService;
        }


        public async Task<DoctorProfileContract?> GetDoctorProfileByIdAsync(long id)
        {
            try
            {
                var doctorDeatils = await _dbContext.Configuration_Doctor
                                .AsNoTracking()
                                .Where(p => p.Id == id && !p.IsDeleted)
                                .Select(p => new DoctorProfileContract
                                {
                                    DoctorId = p.Id,
                                    DoctorCode = p.Code,
                                    DoctorTitle = p.Title,
                                    DoctorFirstName = p.FirstName,
                                    DoctorLastName = p.LastName,
                                    DoctorEducationDegreesStr = p.EducationDegreeIds,
                                    DoctorSpecializedArea = p.SpecializedArea,
                                    ProfilePhotoName = p.ProfilePhotoName,
                                    ProfilePhotoPath = p.ProfilePhotoPath,
                                    DoctorChambersStr = p.ChamberIds,
                                    DoctorYearOfExperiences = p.YearOfExperiences,
                                    DoctorExperiencesStr = p.Experiences,
                                    DoctorBMDCRegNo = p.BMDCRegNo,
                                    DoctorProfessionalSummary = p.ProfessionalSummary,
                                    DoctorRating = p.Rating,
                                    Comments = p.Comments
                                })
                                .FirstOrDefaultAsync();
                if (doctorDeatils is not null)
                {
                    doctorDeatils!.DoctorEducationDegrees = doctorDeatils.DoctorEducationDegreesStr != null
                                            ? _dbContext.Configuration_Education
                                                .AsNoTracking()
                                                .Where(e => doctorDeatils.DoctorEducationDegreesStr
                                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(id => long.Parse(id))
                                                    .Contains(e.Id)).ToList()
                                            : new List<Domain.Entities.Configuration_EducationEntity>();

                    var doctorChambers = await (
                                                from doctor in _dbContext.Configuration_DoctorChamber.AsNoTracking()
                                                //from chamber in _dbContext.Configuration_Chamber
                                                where doctor.DoctorId == id && !doctor.Doctor.IsDeleted //&& doctor.ChamberId == chamber.Id
                                                select new DoctorChamberContract
                                                {
                                                    HospitalId = doctor.HospitalId,
                                                    HospitalName = doctor!.Hospital!.Name,
                                                    DepartmentId = doctor.DepartmentId,
                                                    DepartmentName = doctor!.Department!.Name,
                                                    DepartmentSectionId = doctor.Department.SectionId,
                                                    DepartmentSectionName = doctor.DepartmentSection!.Name,
                                                    IsMainChamber = doctor.IsMainChamber,
                                                    DoctorId = doctor.DoctorId,
                                                    ChamberId = doctor.Id,
                                                    ChamberName = doctor.ChamberName,
                                                    ChamberAddress = doctor.ChamberAddress,

                                                    ChamberCityId = doctor.ChamberCityId,
                                                    ChamberCityName = doctor.City.Name,
                                                    ChamberPostalCode = doctor.ChamberPostalCode,
                                                    ChamberDescription = doctor.ChamberDescription,
                                                    ChamberGoogleAddress = doctor.ChamberGoogleAddress,
                                                    ChamberGoogleRating = doctor.ChamberGoogleRating!,
                                                    ChamberDoctorBookingMobileNos = doctor!.DoctorBookingMobileNos!,
                                                    ChamberHelpline = doctor.Helpline_CallCenter!,
                                                    ChamberEmail = doctor.ChamberEmail!,
                                                    ChamberVisitingHour = doctor.VisitingHour!,
                                                    ChamberOpenDay = "",
                                                    ChamberCloseDay = doctor.ChamberClosedOnDay!,
                                                    ChamberEndTime = doctor.ChamberEndTime!,
                                                    ChamberOtherDoctorsId = doctor.ChamberOtherDoctorsId!,
                                                    DoctorDesignationInChamberId = doctor.DoctorDesignationInChamberId,
                                                    VisitingHour = doctor.VisitingHour,
                                                    Remarks = doctor.Remarks,
                                                    DoctorSpecialization = doctor.DoctorSpecialization,
                                                    IsActive = doctor.IsActive,
                                                }).AsNoTracking().ToListAsync();

                    doctorDeatils!.DoctorChambers = doctorChambers.Where(dc => dc.IsActive == true).ToList() ?? new List<DoctorChamberContract>();
                    doctorDeatils!.DoctorExperiences = doctorChambers ?? new List<DoctorChamberContract>();
                }
                await Task.CompletedTask;
                return doctorDeatils;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<DoctorProfileWithCountContract>> GetDoctorProfilesByUserIdWithPrescriptionCountAsync(long userId,long? PatientId, CancellationToken cancellationToken)
        {
            try
            {
                var eligible = from srm in _dbContext.Smartrx_Master.AsNoTracking()
                                where srm.UserId == userId &&
                                      srm.IsRecommended == true &&
                                      srm.IsApproved == true &&
                                      srm.IsCompleted == true
                                join pd in _dbContext.Smartrx_Doctor.AsNoTracking() on srm.Id equals pd.SmartRxMasterId
                                select new { srm, pd };

                // Optional filter by nullable PatientId
                if (PatientId.HasValue)
                {
                    eligible = eligible.Where(x => x.srm.PatientId == PatientId.Value);
                }

                var query = from e in eligible
                            join d in _dbContext.Configuration_Doctor
                                .AsNoTracking()
                                .Where(d => !d.IsDeleted)
                                on e.pd.DoctorId equals d.Id
                            group new { e.srm, e.pd, d } by new
                            {
                                d.Id,
                                d.Code,
                                d.Title,
                                d.FirstName,
                                d.LastName,
                                d.ProfilePhotoName,
                                d.ProfilePhotoPath,
                                d.BMDCRegNo,
                                d.SpecializedArea,
                                d.Rating
                            }
                    into g
                            select new DoctorProfileWithCountContract
                            {
                                DoctorId = g.Key.Id,
                                DoctorCode = g.Key.Code,
                                DoctorTitle = g.Key.Title,
                                DoctorFirstName = g.Key.FirstName,
                                DoctorLastName = g.Key.LastName,
                                ProfilePhotoName = g.Key.ProfilePhotoName,
                                ProfilePhotoPath = g.Key.ProfilePhotoPath,
                                RegistrationNumber = g.Key.BMDCRegNo,                                
                                DoctorRating = g.Key.Rating,
                                SmartRxCount = g.Select(x => x.srm.Id).Distinct().Count(),
                                PrescriptionCount = g.Select(x => x.pd.PrescriptionId).Distinct().Count()
                            };

                var result = await query.AsNoTracking().ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<DoctorProfileWithCountContract>> GetDoctorProfilesByUserIdWithPagingAsync(long userId, long? patientId, long? doctorId, string? searchKeyword, string? searchColumn, PagingSortingParams? pagingSorting, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (pagingSorting == null)
                {
                    pagingSorting = new PagingSortingParams();
                }

                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;             


                var eligible = from srm in _dbContext.Smartrx_Master.AsNoTracking()
                                where srm.UserId == userId //&&
                                      //srm.IsRecommended == true &&
                                      //srm.IsApproved == true &&
                                      //srm.IsCompleted == true
                                join pd in _dbContext.Smartrx_Doctor.AsNoTracking() on srm.Id equals pd.SmartRxMasterId
                                select new { srm, pd };

                // Optional filter by nullable DoctorId
                if (doctorId.HasValue)
                {
                    eligible = eligible.Where(x => x.pd.DoctorId == doctorId.Value);
                }
                if (patientId.HasValue)
                {
                    eligible = eligible.Where(x => x.srm.PatientId == patientId.Value);
                }
                var baseQuery = from e in eligible
                                join d in _dbContext.Configuration_Doctor
                                    .AsNoTracking()
                                    .Where(d => !d.IsDeleted)
                                    on e.pd.DoctorId equals d.Id
                                group new { e.srm, e.pd, d } by new
                                {
                                    d.Id,
                                    d.Code,
                                    d.Title,
                                    d.FirstName,
                                    d.LastName,
                                    d.ProfilePhotoName,
                                    d.ProfilePhotoPath,
                                    d.BMDCRegNo,
                                    d.Rating
                                }
                    into g
                                select new DoctorProfileWithCountContract
                                {
                                    DoctorId = g.Key.Id,
                                    DoctorCode = g.Key.Code,
                                    DoctorTitle = g.Key.Title,
                                    DoctorFirstName = g.Key.FirstName,
                                    DoctorLastName = g.Key.LastName,
                                    ProfilePhotoName = g.Key.ProfilePhotoName,
                                    ProfilePhotoPath = g.Key.ProfilePhotoPath,
                                    RegistrationNumber = g.Key.BMDCRegNo,
                                    DoctorRating = g.Key.Rating,
                                    SmartRxCount = g.Select(x => x.srm.Id).Distinct().Count(),
                                    PrescriptionCount = g.Select(x => x.pd.PrescriptionId).Distinct().Count()
                                };

                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    var searchTerm = searchKeyword.Trim().ToLower();
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        switch (searchColumn?.ToLower())
                        {
                            case "firstname":
                                baseQuery = baseQuery.Where(x => x.DoctorFirstName != null && x.DoctorFirstName.ToLower().Contains(searchTerm));
                                break;
                            case "lastname":
                                baseQuery = baseQuery.Where(x => x.DoctorLastName != null && x.DoctorLastName.ToLower().Contains(searchTerm));
                                break;
                            case "doctorcode":
                                baseQuery = baseQuery.Where(x => x.DoctorCode != null && x.DoctorCode.ToLower().Contains(searchTerm));
                                break;
                            case "registrationnumber":
                                baseQuery = baseQuery.Where(x => x.RegistrationNumber != null && x.RegistrationNumber.ToLower().Contains(searchTerm));
                                break;
                            case "all":
                            default:
                                baseQuery = baseQuery.Where(x =>
                                    (x.DoctorFirstName != null && x.DoctorFirstName.ToLower().Contains(searchTerm)) ||
                                    (x.DoctorLastName != null && x.DoctorLastName.ToLower().Contains(searchTerm)) ||
                                    (x.DoctorCode != null && x.DoctorCode.ToLower().Contains(searchTerm)) ||
                                    (x.RegistrationNumber != null && x.RegistrationNumber.ToLower().Contains(searchTerm))
                                );
                                break;
                        }
                    }
                }

                // rxType filter if needed in future (kept for symmetry with patient API)
                // Currently SmartRxCount already reflects SmartRx; additional filtering by rxType could be applied here

                var totalRecords = await baseQuery.CountAsync(cancellationToken);

                IQueryable<DoctorProfileWithCountContract> sortedQuery;
                switch (pagingSorting.SortBy?.ToLower())
                {
                    case "firstname":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DoctorFirstName)
                            : baseQuery.OrderBy(x => x.DoctorFirstName);
                        break;
                    case "lastname":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DoctorLastName)
                            : baseQuery.OrderBy(x => x.DoctorLastName);
                        break;
                    case "doctorcode":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DoctorCode)
                            : baseQuery.OrderBy(x => x.DoctorCode);
                        break;
                    case "smartrxcount":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.SmartRxCount)
                            : baseQuery.OrderBy(x => x.SmartRxCount);
                        break;
                    case "rating":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DoctorRating)
                            : baseQuery.OrderBy(x => x.DoctorRating);
                        break;
                    default:
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DoctorFirstName)
                            : baseQuery.OrderBy(x => x.DoctorFirstName);
                        break;
                }

                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<DoctorProfileWithCountContract>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
