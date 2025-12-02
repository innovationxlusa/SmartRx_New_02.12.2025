using Microsoft.EntityFrameworkCore;
using PMSBackend.Application.DTOs;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class BrowseRxRepository : IBrowseRxRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IBaseRepository<Prescription_UserWiseFolderEntity> _userWiseFolderRepository;
        private readonly IBaseRepository<Prescription_UploadEntity> _prescriptionRepository;

        public BrowseRxRepository(PMSDbContext context, IBaseRepository<Prescription_UserWiseFolderEntity> userWiseFolderRepository, IBaseRepository<Prescription_UploadEntity> prescriptionUploadRepository)
        {
            _dbContext = context;
            _userWiseFolderRepository = userWiseFolderRepository;
            _prescriptionRepository = prescriptionUploadRepository;
        }

        public async Task<List<PrescriptionAllListModel>> GetAllFilesAsync(long userId, long? patientId, long? folderId, long? parentFolderId, long? folderHierarchy = 0)
        {
            try
            {

                var result = from u in _dbContext.Prescription_UserWiseFolders.AsNoTracking()
                             join p in _dbContext.Prescription_UploadedPrescription.AsNoTracking()
                             on new { u.UserId, FolderId = u.Id } equals new { p.UserId, FolderId = p.FolderId }
                             where p.UserId == userId && !p.IsDeleted && (patientId == null || p.PatientId == patientId) 
                             && (folderId ==null || u.ParentFolderId==folderId)
                             select new PrescriptionAllListModel
                             {
                                 FileId = p.Id,
                                 PrescriptionCode = p.PrescriptionCode,
                                 UserId = u.UserId,
                                 FolderId = u.Id,
                                 IsExistingPatient = p.IsExistingPatient ?? false,
                                 PatientId = p.PatientId ?? 0,
                                 ParentFolderId = u.ParentFolderId,
                                 FolderHeirarchy = u.FolderHierarchy,
                                 FileName = p.FileName,
                                 FilePath = p.FilePath,
                                 filStoredForThisPrescriptionCount = p.NumberOfFilesStoredForThisPrescription,
                                 FileExtension = p.FileExtension,
                                 IsSmartRxRequested = p.IsSmartRxRequested ?? false,
                                 SmartRxMasterId=p.SmartRxId,
                                 HasRelative = p.HasExistingRelative,
                                 PatientRelativesId = p.RelativePatientIds,
                                 IsSmarted = (p.IsRecommended == true &&
                                              p.IsApproved == true &&
                                              p.IsCompleted == true),
                                 IsWaiting = (p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)),
                                 CreatedById = p.CreatedById,
                                 CreatedDate = p.CreatedDate,
                                 Tag1 = p.Tag1,
                                 Tag2 = p.Tag2,
                                 Tag3 = p.Tag3,
                                 Tag4 = p.Tag4,
                                 Tag5 = p.Tag5
                             };
                var data = await result.ToListAsync();
                await Task.CompletedTask;
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedResult<PrescriptionAllListModel>> GetAllFilesWithPagingAsync(long userId, long? folderId, long? parentFolderId, long? folderHierarchy, PagingSortingParams pagingSorting)
        {
            try
            {
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var baseQuery = from u in _dbContext.Prescription_UserWiseFolders.AsNoTracking()
                                join p in _dbContext.Prescription_UploadedPrescription.AsNoTracking()
                                on new { u.UserId, FolderId = u.Id } equals new { p.UserId, FolderId = p.FolderId }
                                where p.UserId == userId && !p.IsDeleted
                                select new PrescriptionAllListModel
                                {
                                    FileId = p.Id,
                                    PrescriptionCode = p.PrescriptionCode,
                                    PrescriptionDate = p.PrescriptionDate,
                                    UserId = u.UserId,
                                    FolderId = u.Id,
                                    IsExistingPatient = p.IsExistingPatient ?? false,
                                    PatientId = p.PatientId ?? 0,
                                    ParentFolderId = u.ParentFolderId,
                                    FolderHeirarchy = u.FolderHierarchy,
                                    FileName = p.FileName,
                                    FilePath = p.FilePath,
                                    filStoredForThisPrescriptionCount = p.NumberOfFilesStoredForThisPrescription,
                                    FileExtension = p.FileExtension,
                                    IsSmartRxRequested = p.IsSmartRxRequested ?? false,
                                    HasRelative = p.HasExistingRelative,
                                    PatientRelativesId = p.RelativePatientIds,
                                    IsSmarted = (p.IsRecommended == true &&
                                                 p.IsApproved == true &&
                                                 p.IsCompleted == true),
                                    IsWaiting = (p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)),
                                    CreatedById = p.CreatedById,
                                    CreatedDate = p.CreatedDate,
                                    Tag1 = p.Tag1,
                                    Tag2 = p.Tag2,
                                    Tag3 = p.Tag3,
                                    Tag4 = p.Tag4,
                                    Tag5 = p.Tag5
                                };

                // Apply filtering based on folder parameters
                if (folderId.HasValue)

                {
                    baseQuery = baseQuery.Where(x => x.FolderId == folderId.Value);
                }
                if (parentFolderId.HasValue)
                {
                    baseQuery = baseQuery.Where(x => x.ParentFolderId == parentFolderId.Value);
                }
                if (folderHierarchy.HasValue)
                {
                    baseQuery = baseQuery.Where(x => x.FolderHeirarchy == folderHierarchy.Value);
                }

                // Get total count
                var totalRecords = await baseQuery.CountAsync();

                // Apply sorting
                IQueryable<PrescriptionAllListModel> sortedQuery;
                switch (pagingSorting.SortBy?.ToLower())
                {
                    case "filename":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.FileName)
                            : baseQuery.OrderBy(x => x.FileName);
                        break;
                    case "prescriptiondate":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.CreatedDate)
                            : baseQuery.OrderBy(x => x.CreatedDate);
                        break;
                    case "prescriptioncode":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.PrescriptionCode)
                            : baseQuery.OrderBy(x => x.PrescriptionCode);
                        break;

                    default:
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.CreatedDate)
                            : baseQuery.OrderBy(x => x.CreatedDate);
                        break;
                }

                // Apply paging
                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync();

                await Task.CompletedTask;
                return new PaginatedResult<PrescriptionAllListModel>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task GetAllFileListByPatient(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Prescription_UserWiseFolderEntity>> GetAllFoldersAsync(long userId, long? folderId, long? folderHierarchy, long? parentFolderId)
        {
            try
            {
                List<Prescription_UserWiseFolderEntity> result = new List<Prescription_UserWiseFolderEntity>();
                if (folderId is not null && folderHierarchy is not null && parentFolderId is not null)
                {
                    result = await _dbContext.Prescription_UserWiseFolders
                        .AsNoTracking()
                        .Where(data => data.UserId == userId && data.Id == folderId && data.FolderHierarchy == folderHierarchy && data.ParentFolderId == parentFolderId)
                        .ToListAsync();
                }
                else if (folderId is not null && folderHierarchy is not null)
                {
                    result = await _dbContext.Prescription_UserWiseFolders
                        .AsNoTracking()
                        .Where(data => data.UserId == userId && data.Id == folderId && data.FolderHierarchy == folderHierarchy)
                        .ToListAsync();
                }
                else result = await _dbContext.Prescription_UserWiseFolders
                        .AsNoTracking()
                        .Where(data => data.UserId == userId)
                        .ToListAsync();
                await Task.CompletedTask;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<FolderNode>>? GetAllFolderListByUserIdAsync(long userId,long? patientId)
        {
            try
            {
                var flatList = await (from f in _dbContext.Prescription_UserWiseFolders.AsNoTracking()
                                          //  join p in _dbContext.Prescription_UploadedPrescription
                                          //  on new { f.UserId, FolderId = f.Id }
                                          //   equals new { p.UserId, FolderId = p.FolderId }
                                      where f.UserId == userId && (patientId == null || f.PatientId == patientId)
                                      select new FolderNode
                                      {
                                          FolderId = f.Id,
                                          UserId = f.UserId,
                                          ParentFolderId = f.ParentFolderId,
                                          FolderHeirarchy = f.FolderHierarchy,
                                          FolderName = f.FolderName,
                                          CreatedDate=f.CreatedDate,
                                          CreatedDateStr = Convert.ToDateTime(f.CreatedDate).ToString("dd-MM-yyyy"),
                                      }).OrderByDescending(f=>f.CreatedDate).ToListAsync();
                //var groupedFolders = flatList
                //.GroupBy(f => f.FolderId)
                //.ToDictionary(g => g.Key, g => g.ToList());

                return flatList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task GetAllFolderListByPatient(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResult<PatientPrescriptionContract>> GetPatientPrescriptionsWithPagingAsync(PatientPrescriptionSearchContract searchParams)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (searchParams.PagingSorting == null)
                {
                    searchParams.PagingSorting = new PagingSortingParams();
                }

                if (searchParams.PagingSorting.PageNumber <= 0) searchParams.PagingSorting.PageNumber = 1;
                if (searchParams.PagingSorting.PageSize <= 0) searchParams.PagingSorting.PageSize = 10;

                // Base query to get patient prescription data (LEFT JOIN to include all prescriptions)
                var baseQuery = from p in _dbContext.Prescription_UploadedPrescription.AsNoTracking()
                                join patient in _dbContext.Smartrx_PatientProfile.AsNoTracking()
                                    on p.PatientId equals patient.Id into patientJoin
                                from patient in patientJoin.DefaultIfEmpty()
                                where p.UserId == searchParams.UserId && !p.IsDeleted
                                select new
                                {
                                    PatientId = patient != null ? patient.Id : 0,
                                    PatientName = patient != null ? (patient.FirstName + " " + patient.LastName + " " + (patient.NickName ?? "")) : "",
                                    PatientCode = patient != null ? patient.PatientCode : "",
                                    FileId = p.Id,
                                    PrescriptionCode = p.PrescriptionCode,
                                    FileName = p.FileName,
                                    FilePath = p.FilePath,
                                    FileExtension = p.FileExtension,
                                    IsSmartRxRequested = p.IsSmartRxRequested ?? false,
                                    HasRelative = p.HasExistingRelative,
                                    PatientRelativesId = p.RelativePatientIds,
                                    IsSmarted = (p.IsRecommended == true && p.IsApproved == true && p.IsCompleted == true),
                                    IsWaiting = (p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)),
                                    CreatedById = p.CreatedById,
                                    CreatedDate = p.CreatedDate,
                                    Tag1 = p.Tag1,
                                    Tag2 = p.Tag2,
                                    Tag3 = p.Tag3,
                                    Tag4 = p.Tag4,
                                    Tag5 = p.Tag5
                                };

                // Apply search filter only if SearchKeyword is provided
                if (!string.IsNullOrWhiteSpace(searchParams.SearchKeyword))
                {
                    var searchTerm = searchParams.SearchKeyword.Trim().ToLower();

                    // Only apply search if the keyword is not empty after trimming
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        switch (searchParams.SearchColumn?.ToLower())
                        {
                            case "patientname":
                                baseQuery = baseQuery.Where(x => x.PatientName.ToLower().Contains(searchTerm));
                                break;
                            case "prescriptioncode":
                                baseQuery = baseQuery.Where(x => x.PrescriptionCode.ToLower().Contains(searchTerm));
                                break;
                            case "filename":
                                baseQuery = baseQuery.Where(x => x.FileName.ToLower().Contains(searchTerm));
                                break;
                            case "tag1":
                                baseQuery = baseQuery.Where(x => x.Tag1 != null && x.Tag1.ToLower().Contains(searchTerm));
                                break;
                            case "tag2":
                                baseQuery = baseQuery.Where(x => x.Tag2 != null && x.Tag2.ToLower().Contains(searchTerm));
                                break;
                            case "tag3":
                                baseQuery = baseQuery.Where(x => x.Tag3 != null && x.Tag3.ToLower().Contains(searchTerm));
                                break;
                            case "tag4":
                                baseQuery = baseQuery.Where(x => x.Tag4 != null && x.Tag4.ToLower().Contains(searchTerm));
                                break;
                            case "tag5":
                                baseQuery = baseQuery.Where(x => x.Tag5 != null && x.Tag5.ToLower().Contains(searchTerm));
                                break;
                            case "all":
                            default:
                                baseQuery = baseQuery.Where(x =>
                                    x.PatientName.ToLower().Contains(searchTerm) ||
                                    x.PrescriptionCode.ToLower().Contains(searchTerm) ||
                                    x.FileName.ToLower().Contains(searchTerm) ||
                                    (x.Tag1 != null && x.Tag1.ToLower().Contains(searchTerm)) ||
                                    (x.Tag2 != null && x.Tag2.ToLower().Contains(searchTerm)) ||
                                    (x.Tag3 != null && x.Tag3.ToLower().Contains(searchTerm)) ||
                                    (x.Tag4 != null && x.Tag4.ToLower().Contains(searchTerm)) ||
                                    (x.Tag5 != null && x.Tag5.ToLower().Contains(searchTerm)));
                                break;
                        }
                    }
                }

                // Group by patient and get patient-level data (exclude records without a patient from patient grouping)
                var patientGroups = await baseQuery
                    .Where(x => x.PatientId != 0)
                    .GroupBy(x => new { x.PatientId, x.PatientName, x.PatientCode })
                    .Select(g => new
                    {
                        PatientId = g.Key.PatientId,
                        PatientName = g.Key.PatientName,
                        PatientCode = g.Key.PatientCode,
                        TotalPrescriptions = g.Count(),
                        SmartRxCount = g.Count(x => x.IsSmarted),
                        WaitingCount = g.Count(x => x.IsWaiting),
                        FileOnlyCount = g.Count(x => !x.IsSmarted && !x.IsWaiting),
                        LastPrescriptionDate = g.Max(x => x.CreatedDate),
                        Prescriptions = g.Select(x => new PrescriptionContract
                        {
                            FileId = x.FileId,
                            PrescriptionCode = x.PrescriptionCode,
                            PrescriptionDate = x.CreatedDate!.Value.ToString("dd-MM-yyyy") ?? "",
                            IsExistingPatient = true, // Since we're joining with patient table
                            PatientId = x.PatientId,
                            UserId = searchParams.UserId,
                            FolderId = 0, // Not applicable for patient view
                            ParentFolderId = null,
                            FolderHeirarchy = 0,
                            FileName = x.FileName,
                            FilePath = x.FilePath,
                            FilePathList = new List<string> { x.FilePath },
                            FileExtension = x.FileExtension,
                            filStoredForThisPrescriptionCount = 1,
                            IsSmartRxRequested = x.IsSmartRxRequested,
                            HasRelative = x.HasRelative,
                            PatientRelativesId = x.PatientRelativesId,
                            IsSmarted = x.IsSmarted,
                            IsWaiting = x.IsWaiting,

                            CreatedById = x.CreatedById,
                            CreatedDate = x.CreatedDate,
                            CreatedDateStr = x.CreatedDate.Value.ToString("dd-MM-yyyy") ?? "",
                            IsFile = true,
                            Tag1 = x.Tag1,
                            Tag2 = x.Tag2,
                            Tag3 = x.Tag3,
                            Tag4 = x.Tag4,
                            Tag5 = x.Tag5
                        }).ToList()
                    })
                    .ToListAsync();

                // Compute overall counts directly from database for category rows
                var overallTotal = await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => p.UserId == searchParams.UserId && !p.IsDeleted)
                    .CountAsync();

                var overallSmartRx = await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => p.UserId == searchParams.UserId && !p.IsDeleted
                        && p.IsRecommended == true && p.IsApproved == true && p.IsCompleted == true)
                    .CountAsync();

                var overallWaiting = await _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => p.UserId == searchParams.UserId && !p.IsDeleted
                        && p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false))
                    .CountAsync();

                var overallFileOnly = overallTotal - overallSmartRx - overallWaiting;

                // Get total count for paging
                var totalRecords = patientGroups.Where(t => t.SmartRxCount > 0).Count();

                //// Check if we need to add SmartRx default row
                //var hasSmartRxData = patientGroups.Any(x => x.SmartRxCount > 0);
                //if (!hasSmartRxData)
                //{
                //    totalRecords += 1; // Add 1 for SmartRx default row
                //}

                // Check if we need to add waiting list default row
                var hasWaitingListData = patientGroups.Any(x => x.WaitingCount > 0);
                if (hasWaitingListData)
                {
                    totalRecords += 1; // Add 1 for waiting list default row
                }

                // Check if we need to add uncategorized default row
                //var hasUncategorizedData = patientGroups.Any(x => (x.TotalPrescriptions- x.WaitingCount-x.SmartRxCount)>0);
                if (overallFileOnly > 0)
                {
                    totalRecords += 1; // Add 1 for uncategorized default row
                }

                // Apply sorting
                IEnumerable<dynamic> sortedQuery;
                switch (searchParams.PagingSorting.SortBy?.ToLower())
                {
                    case "patientname":
                        sortedQuery = searchParams.PagingSorting.SortDirection.ToLower() == "desc"
                            ? patientGroups.OrderByDescending(x => x.PatientName)
                            : patientGroups.OrderBy(x => x.PatientName);
                        break;
                    case "patientcode":
                        sortedQuery = searchParams.PagingSorting.SortDirection.ToLower() == "desc"
                            ? patientGroups.OrderByDescending(x => x.PatientCode)
                            : patientGroups.OrderBy(x => x.PatientCode);
                        break;
                    case "totalprescriptions":
                        sortedQuery = searchParams.PagingSorting.SortDirection.ToLower() == "desc"
                            ? patientGroups.OrderByDescending(x => x.TotalPrescriptions)
                            : patientGroups.OrderBy(x => x.TotalPrescriptions);
                        break;
                    case "lastprescriptiondate":
                        sortedQuery = searchParams.PagingSorting.SortDirection.ToLower() == "desc"
                            ? patientGroups.OrderByDescending(x => x.LastPrescriptionDate)
                            : patientGroups.OrderBy(x => x.LastPrescriptionDate);
                        break;
                    default:
                        sortedQuery = searchParams.PagingSorting.SortDirection.ToLower() == "desc"
                            ? patientGroups.OrderByDescending(x => x.PatientName)
                            : patientGroups.OrderBy(x => x.PatientName);
                        break;
                }

                // Apply paging
                var pagedData = sortedQuery
                    .Skip((searchParams.PagingSorting.PageNumber - 1) * searchParams.PagingSorting.PageSize)
                    .Take(searchParams.PagingSorting.PageSize)
                    .Select(x => new PatientPrescriptionContract
                    {
                        PatientId = x.PatientId,
                        PatientName = x.PatientName,
                        PatientCode = x.PatientCode,
                        TotalPrescriptions = x.TotalPrescriptions,
                        SmartRxCount = x.SmartRxCount,
                        WaitingCount = x.WaitingCount,
                        FileOnlyCount = x.FileOnlyCount,
                        LastPrescriptionDate = x.LastPrescriptionDate,
                        LastPrescriptionDateStr = x.LastPrescriptionDate?.ToString("dd-MM-yyyy") ?? "",
                        Prescriptions = x.Prescriptions
                    })
                    .ToList();


                var pagedFinalList = new List<PatientPrescriptionContract>();

                //// Check if there's any SmartRx data in the results
                var smartRxData = pagedData.Where(x => x.SmartRxCount > 0).ToList();
                // Add default SmartRx row if no SmartRx data found (populate with real overall counts)
                foreach (var smartRx in smartRxData)
                {
                    pagedFinalList.Add(new PatientPrescriptionContract
                    {
                        PatientId = smartRx.PatientId, // Special ID for SmartRx
                        PatientName = smartRx.PatientName,
                        PatientCode = smartRx.PatientCode,
                        TotalPrescriptions = overallSmartRx,
                        SmartRxCount = overallSmartRx,
                        IsSmartRx = smartRx.IsSmartRx,
                        WaitingCount = 0,
                        FileOnlyCount = 0,
                        LastPrescriptionDate = smartRx.LastPrescriptionDate,
                        LastPrescriptionDateStr = smartRx.LastPrescriptionDateStr,
                        Prescriptions = smartRx.Prescriptions
                    });
                }


                // Check if there's any waiting list data in the results
                hasWaitingListData = pagedData.Any(x => x.WaitingCount > 0);


                // Add default waiting list row if no waiting list data found (populate with real overall counts)

                pagedFinalList.Add(new PatientPrescriptionContract
                {
                    PatientId = -2, // Special ID for Waiting List
                    PatientName = "Waiting for SmartRx",
                    PatientCode = "WAIT-001",
                    TotalPrescriptions = overallWaiting,
                    SmartRxCount = 0,
                    WaitingCount = overallWaiting,
                    IsWaitingList = true,
                    FileOnlyCount = 0,
                    LastPrescriptionDate = null,
                    LastPrescriptionDateStr = "",
                    Prescriptions = new List<PrescriptionContract>()
                });


                // Check if there's any uncategorized data in the results
                //hasUncategorizedData = pagedData.Any(x => (x.TotalPrescriptions - x.WaitingCount - x.SmartRxCount) > 0);

                // Add default uncategorized row if no uncategorized data found (populate with real overall counts)

                pagedFinalList.Add(new PatientPrescriptionContract
                {
                    PatientId = -3, // Special ID for Un-categorized
                    PatientName = "Un-Categorized",
                    PatientCode = "UNCAT-001",
                    TotalPrescriptions = overallFileOnly,
                    SmartRxCount = 0,
                    WaitingCount = 0,
                    FileOnlyCount = overallFileOnly,
                    IsUncategorized = true,
                    LastPrescriptionDate = null,
                    LastPrescriptionDateStr = "",
                    Prescriptions = new List<PrescriptionContract>()
                });


                await Task.CompletedTask;
                return new PaginatedResult<PatientPrescriptionContract>(
                    pagedFinalList,
                    totalRecords,
                    searchParams.PagingSorting.PageNumber,
                    searchParams.PagingSorting.PageSize,
                    searchParams.PagingSorting.SortBy,
                    searchParams.PagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedResult<PrescriptionContract>> GetPatientPrescriptionsByTypeAsync(long userId, long? patientId, string prescriptionType, PagingSortingParams? pagingSorting)
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

                // Start with base prescriptions and apply type filter BEFORE join/projection
                var prescriptions = _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => p.UserId == userId && !p.IsDeleted && (patientId == null || p.PatientId == patientId));

                switch ((prescriptionType ?? "").ToLower())
                {
                    case "smartrx":
                        prescriptions = prescriptions.Where(p => p.IsRecommended == true && p.IsApproved == true && p.IsCompleted == true);
                        break;
                    case "Pending":
                    case "waiting":
                    case "waitinglist":
                        prescriptions = prescriptions.Where(p => p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false));
                        break;
                    case "uncategorized":
                    case "fileonly":
                        prescriptions = prescriptions.Where(p =>
                            ((p.IsSmartRxRequested == null|| p.IsSmartRxRequested==false) && (p.IsRecommended == null || p.IsRecommended == false) && (p.IsApproved == null || p.IsApproved == false) && (p.IsCompleted == null || p.IsCompleted == false))                            
                        );
                        break;
                    case "":
                        break;
                    default:
                        throw new ArgumentException($"Invalid prescription type: {prescriptionType}. Valid types are: smartrx, waiting, uncategorized");
                }

                // LEFT JOIN to patient profile and projection
                var baseQuery = from p in prescriptions
                                join patient in _dbContext.Smartrx_PatientProfile.AsNoTracking()
                                  on p.PatientId equals patient.Id into patientJoin
                                from patient in patientJoin.DefaultIfEmpty()
                                join smartRx in _dbContext.Smartrx_Master.AsNoTracking()
                                    on p.Id equals smartRx.PrescriptionId into smartRxJoin
                                from smartRx in smartRxJoin.DefaultIfEmpty()                              
                                select new PrescriptionContract
                                {
                                    FileId = p.Id,
                                    PrescriptionCode = p.PrescriptionCode,
                                    PrescriptionDate = p.PrescriptionDate.ToString("dd-MM-yyyy") ?? "",
                                    SmartRxMasterId = smartRx != null ? smartRx.Id : p.SmartRxId,
                                    IsExistingPatient = patient != null,
                                    PatientId = p.PatientId ?? 0,
                                    UserId = userId,
                                    FolderId = p.FolderId,
                                    ParentFolderId = null,
                                    FolderHeirarchy = 0,
                                    FileName = p.FileName,
                                    FilePath = p.FilePath,
                                    FilePathList = new List<string> { p.FilePath },
                                    FileExtension = p.FileExtension,
                                    filStoredForThisPrescriptionCount = p.NumberOfFilesStoredForThisPrescription,
                                    IsSmartRxRequested = p.IsSmartRxRequested ?? false,
                                    HasRelative = p.HasExistingRelative,
                                    PatientRelativesId = p.RelativePatientIds,
                                    IsSmarted = (p.IsRecommended == true && p.IsApproved == true && p.IsCompleted == true),
                                    IsWaiting = (p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)),
                                    CreatedById = p.CreatedById,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDateStr = p.CreatedDate!.Value.ToString("dd-MM-yyyy") ?? "",
                                    IsFile = true,
                                    Tag1 = p.Tag1,
                                    Tag2 = p.Tag2,
                                    Tag3 = p.Tag3,
                                    Tag4 = p.Tag4,
                                    Tag5 = p.Tag5
                                };

                // total count
                var totalRecords = await baseQuery.CountAsync();

                // sorting
                IQueryable<PrescriptionContract> sortedQuery;
                switch ((pagingSorting.SortBy ?? "createddate").ToLower())
                {
                    case "filename":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.FileName)
                            : baseQuery.OrderBy(x => x.FileName);
                        break;
                    case "prescriptioncode":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.PrescriptionCode)
                            : baseQuery.OrderBy(x => x.PrescriptionCode);
                        break;
                    case "createddate":
                    default:
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.CreatedDate)
                            : baseQuery.OrderBy(x => x.CreatedDate);
                        break;
                }

                // paging
                var pagedData = await sortedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync();

                await Task.CompletedTask;
                return new PaginatedResult<PrescriptionContract>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}