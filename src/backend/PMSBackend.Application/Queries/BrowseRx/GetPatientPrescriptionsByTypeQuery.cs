using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.BrowseRx
{
    public class GetPatientPrescriptionsByTypeQuery : IRequest<PaginatedResult<PrescriptionDTO>>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
        public string PrescriptionType { get; set; } = string.Empty; // "smartrx", "waiting", "uncategorized"
        public PagingSortingParams? PagingSorting { get; set; } = new PagingSortingParams(); // Default initialization for Swagger
    }

    public class GetPatientPrescriptionsByTypeQueryHandler : IRequestHandler<GetPatientPrescriptionsByTypeQuery, PaginatedResult<PrescriptionDTO>>
    {
        private readonly IBrowseRxRepository _browseRxRepository;

        public GetPatientPrescriptionsByTypeQueryHandler(IBrowseRxRepository browseRxRepository)
        {
            _browseRxRepository = browseRxRepository;
        }

        public async Task<PaginatedResult<PrescriptionDTO>> Handle(GetPatientPrescriptionsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (request.PagingSorting == null)
                {
                    request.PagingSorting = new PagingSortingParams();
                }

                var contractResult = await _browseRxRepository.GetPatientPrescriptionsByTypeAsync(
                    request.UserId, 
                    request.PatientId, 
                    request.PrescriptionType,
                    request.PagingSorting);

                // Convert Contract to DTO
                var dtoData = contractResult.Data.Select(p => new PrescriptionDTO
                {
                    FileId = p.FileId,
                    PrescriptionCode = p.PrescriptionCode,
                    PrescriptionDate = p.PrescriptionDate,
                    SmartRxMasterId = p.SmartRxMasterId,
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
                }).ToList();

                var dtoResult = new PaginatedResult<PrescriptionDTO>(
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
