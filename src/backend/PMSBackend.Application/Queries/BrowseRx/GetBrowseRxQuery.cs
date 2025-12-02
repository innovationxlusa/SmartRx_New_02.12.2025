using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Net.NetworkInformation;
using System.Linq;

namespace PMSBackend.Application.Queries.BrowseRx
{
    public class GetBrowseRxQuery : IRequest<FolderNodeDTO>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
        public long? FolderId { get; set; }
        public long? FolderHeirarchy { get; set; }
        public long? ParentFolderId { get; set; }
        public string? CurrentFolderPath { get; set; }
        public PagingSortingParams PagingSorting { get; set; } = new PagingSortingParams() { SortBy = "filename" };
    }

    public class GetBrowseRxQueryHandler : IRequestHandler<GetBrowseRxQuery, FolderNodeDTO>
    {
        private readonly IBrowseRxRepository _browseRxRepository;
        public GetBrowseRxQueryHandler(IBrowseRxRepository browseRxRepository)
        {
            _browseRxRepository = browseRxRepository;
        }
        public async Task<FolderNodeDTO?> Handle(GetBrowseRxQuery request, CancellationToken cancellationToken)
        {
            //BrowseRxDTO dto = new BrowseRxDTO();
            try
            {
                //var responseResult = new UserDetailsResponseDTO();
                //UserWiseFolderDTO folder=new UserWiseFolderDTO();
                //List<UserWiseFolderDTO> folders = new List<UserWiseFolderDTO>();
                var folderNodeList = new List<FolderNodeDTO>();
                var folderlist = await _browseRxRepository.GetAllFolderListByUserIdAsync(request.UserId,request.PatientId);
                if (folderlist != null)
                {
                    folderNodeList = folderlist.Select(p => new FolderNodeDTO
                    {
                        FolderId = p.FolderId,
                        UserId = p.UserId,
                        FolderOrFileName = p.FolderName,
                        ParentFolderId = p.ParentFolderId,
                        FolderHeirarchy = p.FolderHeirarchy,
                        CreatedDate = p.CreatedDate,
                        CreatedDateStr = p.CreatedDateStr

                    }).ToList();
                }

                List<PrescriptionDTO>? prescriptions = new List<PrescriptionDTO>();
                PaginatedResult<PrescriptionDTO>? paginatedPrescriptions = null;              
                var allTypeFiles = await _browseRxRepository.GetAllFilesAsync(request.UserId, request.PatientId, request.FolderId, request.ParentFolderId, request.FolderHeirarchy ?? 0);
                if (allTypeFiles is not null)
                {
                    var filteredFiles = request.PatientId.HasValue
                        ? allTypeFiles.Where(f => f.PatientId == request.PatientId.Value)
                        : allTypeFiles;

                    foreach (var file in filteredFiles)
                    {
                        var prescriptionDto = ConvertToPrescriptionDTO(file, request.CurrentFolderPath);
                        prescriptions.Add(prescriptionDto);
                    }
                }
                return BuildFolderTree(null, folderNodeList, prescriptions, request.PagingSorting, paginatedPrescriptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private PrescriptionDTO ConvertToPrescriptionDTO(PrescriptionAllListModel file, string? currentFolderPath)
        {
            PrescriptionDTO prescriptionDto = new PrescriptionDTO();
            prescriptionDto.FileId = file.FileId;
            prescriptionDto.PrescriptionCode = file.PrescriptionCode;
            prescriptionDto.UserId = file.UserId;
            prescriptionDto.FolderId = file.FolderId;
            prescriptionDto.ParentFolderId = file.ParentFolderId;
            prescriptionDto.IsExistingPatient = file.IsExistingPatient;
            prescriptionDto.PatientId = file.PatientId;
            prescriptionDto.FileName = file.FileName;
            prescriptionDto.FilePath = file.FilePath;

            var code = file.PrescriptionCode?.Trim();
            var files = Directory.GetFiles(currentFolderPath ?? "")
                .Where(f => Path.GetFileName(f)
                    .StartsWith(code, StringComparison.OrdinalIgnoreCase))
                .Select(Path.GetFileName)
                .ToList();

            var filePathList = files.Select(f => Path.Combine("files", f)).ToList();
            prescriptionDto.FilePathList = filePathList;
            prescriptionDto.filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount;
            prescriptionDto.FileExtension = file.FileExtension;
            prescriptionDto.FolderHeirarchy = file.FolderHeirarchy;
            prescriptionDto.HasRelative = file.HasRelative;
            prescriptionDto.PatientRelativesId = file.PatientRelativesId;
            prescriptionDto.IsSmartRxRequested = file.IsSmartRxRequested;
            prescriptionDto.SmartRxMasterId = file.SmartRxMasterId;
            prescriptionDto.IsSmarted = file.IsSmarted;
            prescriptionDto.IsWaiting = file.IsWaiting;
            prescriptionDto.CreatedById = file.CreatedById;
            prescriptionDto.CreatedDate = file.CreatedDate;
            prescriptionDto.CreatedDateStr = Convert.ToDateTime(file.CreatedDate).ToString("dd-MM-yyyy");
            prescriptionDto.IsFile = true;
            prescriptionDto.Tag1 = file.Tag1;
            prescriptionDto.Tag2 = file.Tag2;
            prescriptionDto.Tag3 = file.Tag3;
            prescriptionDto.Tag4 = file.Tag4;
            prescriptionDto.Tag5 = file.Tag5;

            return prescriptionDto;
        }

        private FolderNodeDTO BuildFolderTree(long? parentFolderId, List<FolderNodeDTO> allFolders, List<PrescriptionDTO> allPrescriptions, PagingSortingParams paging, PaginatedResult<PrescriptionDTO>? paginatedPrescriptions = null)
        {
            FolderNodeDTO rootFolder = new FolderNodeDTO();
            if (!allFolders.Any() && !allPrescriptions.Any())
            {
                return null!;
            }

            if (parentFolderId is null)
            {
                rootFolder = allFolders.FirstOrDefault(f => f.ParentFolderId == null && f.FolderHeirarchy == 0)
                    ?? new FolderNodeDTO
                    {
                        FolderId = 0,
                        ParentFolderId = null,
                        FolderHeirarchy = 0,
                        FolderOrFileName = "Root",
                        UserId = allPrescriptions.FirstOrDefault()?.UserId ?? 0,
                        CreatedDate = DateTime.UtcNow,
                        CreatedDateStr = DateTime.UtcNow.ToString("dd-MM-yyyy"),
                        TotalPrescriptionCount = allPrescriptions.Count()
                    };
                rootFolder.TotalPrescriptionCount = allPrescriptions.Count();
            }
            else
            {
                rootFolder = allFolders.FirstOrDefault(f => f.FolderId == parentFolderId)
                    ?? new FolderNodeDTO
                    {
                        FolderId = parentFolderId.Value,
                        ParentFolderId = null,
                        FolderHeirarchy = 0,
                        FolderOrFileName = "Folder",
                        UserId = allPrescriptions.FirstOrDefault()?.UserId ?? 0,
                        CreatedDate = DateTime.UtcNow,
                        CreatedDateStr = DateTime.UtcNow.ToString("dd-MM-yyyy"),
                        TotalPrescriptionCount = allPrescriptions.Count(p => p.FolderId == parentFolderId)
                    };
            }
            FolderNodeDTO root = new FolderNodeDTO
            {
                IsFolder=true,
                FolderId = rootFolder.FolderId,
                ParentFolderId = rootFolder.ParentFolderId,
                FolderHeirarchy = rootFolder.FolderHeirarchy,
                FolderOrFileName = rootFolder.FolderOrFileName,
                UserId = rootFolder.UserId,
                CreatedDate = rootFolder.CreatedDate,
                CreatedDateStr = rootFolder.CreatedDateStr,
                Children = new PaginatedResult<FolderNodeDTO>(),
                TotalPrescriptionCount = rootFolder.TotalPrescriptionCount,
                //PaginatedPrescriptionList = paginatedPrescriptions
            };
            List<FolderNodeDTO> childNode = new List<FolderNodeDTO>();

            if (parentFolderId != null)
            {
                var folderNodeFiles = allPrescriptions
                    .Where(file => file.FolderId == parentFolderId)
                    .Select(file => new FolderNodeDTO
                    {
                        IsFolder = false,
                        IsFile = true,
                        FolderId = file.FolderId,
                        FileId = file.FileId,
                        FolderOrFileName=file.FileName,
                        PrescriptionCode = file.PrescriptionCode,
                        IsExistingPatient = file.IsExistingPatient,
                        PatientId = file.PatientId,
                        UserId = file.UserId,
                        ParentFolderId = file.ParentFolderId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                        FilePathList = file.FilePathList,
                        filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount,
                        FileExtension = file.FileExtension,
                        IsSmartRxRequested = file.IsSmartRxRequested,
                        SmartRxMasterId=file.SmartRxMasterId,
                        HasRelative = file.HasRelative,
                        PatientRelativesId = file.PatientRelativesId,
                        IsSmarted = file.IsSmarted,
                        IsWaiting = file.IsWaiting,
                        CreatedById = file.CreatedById,
                        CreatedDate = file.CreatedDate,
                        CreatedDateStr = Convert.ToDateTime(file.CreatedDate).ToString("dd-MM-yyyy"),                      
                        Tag1 = file.Tag1,
                        Tag2 = file.Tag2,
                        Tag3 = file.Tag3,
                        Tag4 = file.Tag4,
                        Tag5 = file.Tag5
                    }).ToList();
                childNode.AddRange(folderNodeFiles);

            }
            else
            {
                var folderNodeFiles = allPrescriptions
                    .Where(file => file.FolderHeirarchy == 0 && file.ParentFolderId == null)
                    .Select(file => new FolderNodeDTO
                    {
                        IsFolder = false,
                        IsFile = true,
                        FolderId = file.FolderId,
                        FileId = file.FileId,
                        PrescriptionCode = file.PrescriptionCode,
                        IsExistingPatient = file.IsExistingPatient,
                        PatientId = file.PatientId,
                        UserId = file.UserId,
                        ParentFolderId = file.ParentFolderId,
                        FolderOrFileName=file.FileName,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                        FilePathList = file.FilePathList,
                        filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount,
                        FileExtension = file.FileExtension,
                        IsSmartRxRequested = file.IsSmartRxRequested,
                        SmartRxMasterId = file.SmartRxMasterId,
                        HasRelative = file.HasRelative,
                        PatientRelativesId = file.PatientRelativesId,
                        IsSmarted = file.IsSmarted,
                        IsWaiting = file.IsWaiting,
                        CreatedById = file.CreatedById,
                        CreatedDate = file.CreatedDate,
                        CreatedDateStr = Convert.ToDateTime(file.CreatedDate).ToString("dd-MM-yyyy"),                       
                        Tag1 = file.Tag1,
                        Tag2 = file.Tag2,
                        Tag3 = file.Tag3,
                        Tag4 = file.Tag4,
                        Tag5 = file.Tag5
                    }).ToList();
                childNode.AddRange(folderNodeFiles);

            }

            List<FolderNodeDTO>? childrenFolders = new List<FolderNodeDTO>();
            if (parentFolderId is null)
            {
                childrenFolders = allFolders.Where(f => f.ParentFolderId == root.FolderId).ToList();
            }
            else childrenFolders = allFolders.Where(f => f.ParentFolderId == parentFolderId).ToList();

            if(childrenFolders.Count==0)
            {
                // Sort children: folders first (descending by createdDate), then files (descending by createdDate)
                var sortedChildrens = childNode
                    .OrderBy(x => x.IsFolder ? 0 : 1) // Folders first (0), files second (1)
                    .ThenByDescending(x => x.CreatedDate) // Then sort by creation date descending (newest first)
                    .ToList();

                var totals = sortedChildrens.Count();
                var pages = sortedChildrens.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                root.Children = new PaginatedResult<FolderNodeDTO>(pages, totals, paging.PageNumber, paging.PageSize, paging.SortBy, paging.SortDirection, null,null,null,null,null);
                return root;
            }

            foreach (var folder in childrenFolders)
            {
                var childNodesFolder = BuildFolderTree(folder.FolderId, allFolders, allPrescriptions, paging, paginatedPrescriptions);

                childNode.Add(childNodesFolder);
            }

            // Sort children: folders first (descending by createdDate), then files (descending by createdDate)
            var sortedChildren = childNode
                .OrderBy(x => x.IsFolder ? 0 : 1) // Folders first (0), files second (1)
                .ThenByDescending(x => x.CreatedDate) // Then sort by creation date descending (newest first)
                .ToList();

            var total = sortedChildren.Count();
            var page = sortedChildren.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToList();

            root.Children = new PaginatedResult<FolderNodeDTO>(page, total, paging.PageNumber, paging.PageSize, paging.SortBy, paging.SortDirection, null, null, null, null, null);

            return root;
        }

    }
}