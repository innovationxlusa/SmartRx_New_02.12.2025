using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.BrowseRx
{
    public class GetPatientPrescriptionsQuery : IRequest<PaginatedResult<PatientPrescriptionDTO>>
    {
        public PatientPrescriptionSearchContract? SearchParams { get; set; }
    }

    public class GetPatientPrescriptionsQueryHandler : IRequestHandler<GetPatientPrescriptionsQuery, PaginatedResult<PatientPrescriptionDTO>>
    {
        private readonly IBrowseRxRepository _browseRxRepository;

        public GetPatientPrescriptionsQueryHandler(IBrowseRxRepository browseRxRepository)
        {
            _browseRxRepository = browseRxRepository;
        }

        public async Task<PaginatedResult<PatientPrescriptionDTO>> Handle(GetPatientPrescriptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure search parameters are properly initialized
                if (request.SearchParams == null)
                {
                    request.SearchParams = new PatientPrescriptionSearchContract
                    {
                        PagingSorting = new PagingSortingParams()
                    };
                }
                
                // Initialize PagingSorting if null
                if (request.SearchParams.PagingSorting == null)
                {
                    request.SearchParams.PagingSorting = new PagingSortingParams();
                }
                
                // If SearchKeyword is provided but empty or whitespace, set it to null
                if (string.IsNullOrWhiteSpace(request.SearchParams.SearchKeyword))
                {
                    request.SearchParams.SearchKeyword = null;
                }
                
                // Set default SearchColumn if not provided
                if (string.IsNullOrWhiteSpace(request.SearchParams.SearchColumn))
                {
                    request.SearchParams.SearchColumn = "all";
                }

                var contractResult = await _browseRxRepository.GetPatientPrescriptionsWithPagingAsync(request.SearchParams);
                
                // Convert Contract to DTO
                var dtoData = contractResult.Data.Select(contract => new PatientPrescriptionDTO
                {
                    PatientId = contract.PatientId,
                    PatientName = contract.PatientName,
                    PatientCode = contract.PatientCode,
                    TotalPrescriptions = contract.TotalPrescriptions,
                    SmartRxCount = contract.SmartRxCount,
                    WaitingCount = contract.WaitingCount,
                    FileOnlyCount = contract.FileOnlyCount,
                    LastPrescriptionDate = contract.LastPrescriptionDate,
                    LastPrescriptionDateStr = contract.LastPrescriptionDateStr,
                    IsUncategorized=contract.IsUncategorized,
                    IsWaiting=contract.IsWaitingList,
                    IsSmartRx=contract.IsSmartRx,
                    Prescriptions = contract.Prescriptions.Select(p => new PrescriptionDTO
                    {
                        FileId = p.FileId,
                        PrescriptionCode = p.PrescriptionCode,
                        PrescriptionDate = p.PrescriptionDate,
                        IsExistingPatient = p.IsExistingPatient,
                        PatientId = p.PatientId,
                        UserId = p.UserId,
                        FolderId = p.FolderId,
                        ParentFolderId = p.ParentFolderId,
                        FolderHeirarchy = p.FolderHeirarchy,
                        FileName = p.FileName,
                        FilePath = p.FilePath,
                        FilePathList = p.FilePathList,
                        FileExtension = p.FileExtension,
                        filStoredForThisPrescriptionCount = p.filStoredForThisPrescriptionCount,
                        IsSmartRxRequested = p.IsSmartRxRequested,
                        HasRelative = p.HasRelative,
                        PatientRelativesId = p.PatientRelativesId,
                        IsSmarted = p.IsSmarted,
                        IsWaiting = p.IsWaiting,
                        CreatedById = p.CreatedById,
                        CreatedDate = p.CreatedDate,
                        CreatedDateStr = p.CreatedDateStr,
                        IsFile = p.IsFile,
                        Tag1 = p.Tag1,
                        Tag2 = p.Tag2,
                        Tag3 = p.Tag3,
                        Tag4 = p.Tag4,
                        Tag5 = p.Tag5
                    }).ToList()
                }).ToList();

                var dtoResult = new PaginatedResult<PatientPrescriptionDTO>(
                    dtoData,
                    contractResult.TotalRecords,
                    contractResult.PageNumber,
                    contractResult.PageSize,
                    contractResult.SortBy,
                    contractResult.SortDirection,
                    contractResult.message, null, null, null, null);

                await Task.CompletedTask;
                return dtoResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
