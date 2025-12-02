using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Globalization;
using System.Net.NetworkInformation;

namespace PMSBackend.Application.Queries.BrowseRx
{
    public class GetBrowseRxQuery1 : IRequest<FolderNodeDTO>
    {
        public long UserId { get; set; }
        public long? FolderId { get; set; }
        public long? FolderHeirarchy { get; set; }
        public long? ParentFolderId { get; set; }
        public string? CurrentFolderPath { get; set; }
        public PagingSortingParams PagingSorting { get; set; } = new PagingSortingParams() { SortBy = "filename" };
    }

    //public class GetBrowseRxQuery1Handler : IRequestHandler<GetBrowseRxQuery, FolderNodeDTO>
    //{
    //    private readonly IBrowseRxRepository _browseRxRepository;
    //    public GetBrowseRxQuery1Handler(IBrowseRxRepository browseRxRepository)
    //    {
    //        _browseRxRepository = browseRxRepository;
    //    }
    //    public async Task<FolderNodeDTO?> Handle(GetBrowseRxQuery request, CancellationToken cancellationToken)
    //    {
    //        //BrowseRxDTO dto = new BrowseRxDTO();
    //        try
    //        {
    //            //var responseResult = new UserDetailsResponseDTO();
    //            //UserWiseFolderDTO folder=new UserWiseFolderDTO();
    //            //List<UserWiseFolderDTO> folders = new List<UserWiseFolderDTO>();
    //            var folderNodeList = new List<FolderNodeDTO>();
    //            var folderlist = await _browseRxRepository.GetAllFolderListByUserIdAsync(request.UserId);
    //            if (folderlist != null)
    //            {
    //                folderNodeList = folderlist.Select(p => new FolderNodeDTO
    //                {
    //                    IsFolder = true,
    //                    FolderId = p.FolderId,
    //                    UserId = p.UserId,
    //                    FolderOrFileName = p.FolderName,
    //                    ParentFolderId = p.ParentFolderId,
    //                    FolderHeirarchy = p.FolderHeirarchy,
    //                    CreatedDateStr = p.CreatedDateStr

    //                }).ToList();
    //            }

    //            List<PrescriptionDTO>? prescriptions = new List<PrescriptionDTO>();
    //            PaginatedResult<PrescriptionDTO>? paginatedPrescriptions = null;

    //            var allTypeFiles = await _browseRxRepository.GetAllFilesAsync(request.UserId, request.PatientId, request.FolderId, request.ParentFolderId, request.FolderHeirarchy ?? 0);
    //            if (allTypeFiles is not null)
    //            {
    //                foreach (var file in allTypeFiles)
    //                {
    //                    var prescriptionDto = ConvertToPrescriptionDTO(file, request.CurrentFolderPath);
    //                    prescriptions.Add(prescriptionDto);
    //                }
    //            }
    //            //List<FolderNodeDTO>? childrenFolders = new List<FolderNodeDTO>();
    //            //if (parentFolderId is null)
    //            //{
    //            //    childrenFolders = allFolders.Where(f => f.ParentFolderId == root.FolderId).ToList();
    //            //}
    //            //else childrenFolders = allFolders.Where(f => f.ParentFolderId == parentFolderId).ToList();
    //            //foreach (var folder in childrenFolders)
    //            //{
    //            //    var childNode = BuildFolderTree(folder.FolderId, allFolders, allPrescriptions, paginatedPrescriptions);
    //            //    root.Children.Add(childNode);
    //            //}

    //            var rootNode = BuildFolderTree(null, folderNodeList, prescriptions, paginatedPrescriptions, request.PagingSorting ?? new PagingSortingParams(), true, new HashSet<long>());


    //            return rootNode;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    private PrescriptionDTO ConvertToPrescriptionDTO(PrescriptionAllListModel file, string? currentFolderPath)
    //    {
    //        PrescriptionDTO prescriptionDto = new PrescriptionDTO();
    //        prescriptionDto.FileId = file.FileId;
    //        prescriptionDto.PrescriptionCode = file.PrescriptionCode;
    //        prescriptionDto.UserId = file.UserId;
    //        prescriptionDto.FolderId = file.FolderId;
    //        prescriptionDto.ParentFolderId = file.ParentFolderId;
    //        prescriptionDto.IsExistingPatient = file.IsExistingPatient;
    //        prescriptionDto.PatientId = file.PatientId;
    //        prescriptionDto.FileName = file.FileName;
    //        prescriptionDto.FilePath = file.FilePath;

    //        var code = file.PrescriptionCode?.Trim();
    //        var files = Directory.GetFiles(currentFolderPath ?? "")
    //            .Where(f => Path.GetFileName(f)
    //                .StartsWith(code, StringComparison.OrdinalIgnoreCase))
    //            .Select(Path.GetFileName)
    //            .ToList();

    //        var filePathList = files.Select(f => Path.Combine("files", f)).ToList();
    //        prescriptionDto.FilePathList = filePathList;
    //        prescriptionDto.filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount;
    //        prescriptionDto.FileExtension = file.FileExtension;
    //        prescriptionDto.FolderHeirarchy = file.FolderHeirarchy;
    //        prescriptionDto.HasRelative = file.HasRelative;
    //        prescriptionDto.PatientRelativesId = file.PatientRelativesId;
    //        prescriptionDto.IsSmartRxRequested = file.IsSmartRxRequested;
    //        prescriptionDto.IsSmarted = file.IsSmarted;
    //        prescriptionDto.IsWaiting = file.IsWaiting;
    //        prescriptionDto.CreatedById = file.CreatedById;
    //        prescriptionDto.CreatedDate = file.CreatedDate;
    //        prescriptionDto.CreatedDateStr = Convert.ToDateTime(file.CreatedDate).ToString("dd-MM-yyyy");
    //        prescriptionDto.IsFile = true;
    //        prescriptionDto.Tag1 = file.Tag1;
    //        prescriptionDto.Tag2 = file.Tag2;
    //        prescriptionDto.Tag3 = file.Tag3;
    //        prescriptionDto.Tag4 = file.Tag4;
    //        prescriptionDto.Tag5 = file.Tag5;

    //        return prescriptionDto;
    //    }

    //    private FolderNodeDTO BuildFolderTree(long? parentFolderId, List<FolderNodeDTO> allFolders, List<PrescriptionDTO> allPrescriptions, PaginatedResult<PrescriptionDTO>? paginatedPrescriptions = null, PagingSortingParams? paging = null, bool applyCombinedForThisNode = false, HashSet<long>? visited = null)
    //    {
    //        //visited ??= new HashSet<long>();
    //        FolderNodeDTO rootFolder = new FolderNodeDTO();
    //        if (parentFolderId is null)
    //        {
    //            rootFolder = allFolders.First(f => f.ParentFolderId == null && f.FolderHeirarchy == 0);
    //            rootFolder.TotalPrescriptionCount = allPrescriptions.Where(p => p.ParentFolderId == null && p.FolderHeirarchy == 0).Count();
    //        }
    //        else
    //        {
    //            rootFolder = allFolders.First(f => f.FolderId == parentFolderId);
    //            rootFolder.TotalPrescriptionCount = allPrescriptions.Where(p => p.FolderId == parentFolderId && p.FolderHeirarchy > 0).Count();
    //        }

    //        //// Cycle/self-guard: if we've already visited this folder, return a minimal node to prevent infinite recursion
    //        if (visited.Contains(rootFolder.FolderId))
    //        {
    //            return new FolderNodeDTO
    //            {
    //                IsFolder = true,
    //                FolderId = rootFolder.FolderId,
    //                ParentFolderId = rootFolder.ParentFolderId,
    //                FolderHeirarchy = rootFolder.FolderHeirarchy,
    //                FolderOrFileName = rootFolder.FolderOrFileName,
    //                UserId = rootFolder.UserId,
    //                CreatedDateStr = rootFolder.CreatedDateStr,
    //                TotalPrescriptionCount = rootFolder.TotalPrescriptionCount
    //            };
    //        }
    //        visited.Add(rootFolder.FolderId);
    //        FolderNodeDTO root = new FolderNodeDTO
    //        {
    //            IsFolder = true,
    //            FolderId = rootFolder.FolderId,
    //            ParentFolderId = rootFolder.ParentFolderId,
    //            FolderHeirarchy = rootFolder.FolderHeirarchy,
    //            FolderOrFileName = rootFolder.FolderOrFileName,
    //            UserId = rootFolder.UserId,
    //            CreatedDateStr = rootFolder.CreatedDateStr,
    //            TotalPrescriptionCount = rootFolder.TotalPrescriptionCount,
    //            //PaginatedPrescriptionList = paginatedPrescriptions
    //        };
    //        List<FolderNodeDTO> childNode = new List<FolderNodeDTO>();
    //        List<FolderNodeDTO>? childrenFolders = new List<FolderNodeDTO>();
    //        if (parentFolderId is null)
    //        {
    //            childrenFolders = allFolders.Where(f => f.ParentFolderId == root.FolderId && f.FolderId != root.FolderId).ToList();
    //        }
    //        else
    //        {
    //            //childrenFolders = allFolders.Where(f => f.ParentFolderId == parentFolderId).ToList();
    //            childrenFolders = parentFolderId is null
    //                        ? allFolders.Where(f => f.ParentFolderId == root.FolderId && f.FolderId != root.FolderId).ToList()
    //                        : allFolders.Where(f => f.ParentFolderId == parentFolderId && f.FolderId != parentFolderId).ToList();
    //        }
    //        if (parentFolderId != null)
    //        {
    //            var folderNodeFiles = allPrescriptions
    //                .Where(file => file.ParentFolderId == parentFolderId)
    //                .Select(file => new FolderNodeDTO
    //                {
    //                    IsFolder = false,
    //                    IsFile = true,
    //                    FolderId = file.FolderId,
    //                    FileId = file.FileId,
    //                    FolderOrFileName = file.FileName,
    //                    CreatedDate = file.CreatedDate,
    //                    CreatedDateStr = file.CreatedDateStr,
    //                    ParentFolderId = file.ParentFolderId,
    //                    UserId = file.UserId,
    //                    PrescriptionCode = file.PrescriptionCode,
    //                    IsExistingPatient = file.IsExistingPatient,
    //                    PatientId = file.PatientId,
    //                    FileName = file.FileName,
    //                    FilePath = file.FilePath,
    //                    FilePathList = file.FilePathList,
    //                    filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount,
    //                    FileExtension = file.FileExtension,
    //                    IsSmartRxRequested = file.IsSmartRxRequested,
    //                    HasRelative = file.HasRelative,
    //                    PatientRelativesId = file.PatientRelativesId,
    //                    IsSmarted = file.IsSmarted,
    //                    IsWaiting = file.IsWaiting,
    //                    CreatedById = file.CreatedById,
    //                    Tag1 = file.Tag1,
    //                    Tag2 = file.Tag2,
    //                    Tag3 = file.Tag3,
    //                    Tag4 = file.Tag4,
    //                    Tag5 = file.Tag5
    //                }).ToList();
    //            childNode.AddRange(folderNodeFiles);
    //        }
    //        else
    //        {
    //            var folderNodeFiles = allPrescriptions
    //                .Where(file => file.FolderHeirarchy == 0 && file.ParentFolderId == null)
    //                .Select(file => new FolderNodeDTO
    //                {
    //                    IsFolder = false,
    //                    IsFile = true,
    //                    FolderId = file.FolderId,
    //                    FileId = file.FileId,
    //                    FolderOrFileName = file.FileName,
    //                    CreatedDate = file.CreatedDate,
    //                    CreatedDateStr = Convert.ToDateTime(file.CreatedDate).ToString("dd-MM-yyyy"),
    //                    ParentFolderId = file.ParentFolderId,
    //                    UserId = file.UserId,
    //                    PrescriptionCode = file.PrescriptionCode,
    //                    IsExistingPatient = file.IsExistingPatient,
    //                    PatientId = file.PatientId,
    //                    FileName = file.FileName,
    //                    FilePath = file.FilePath,
    //                    FilePathList = file.FilePathList,
    //                    filStoredForThisPrescriptionCount = file.filStoredForThisPrescriptionCount,
    //                    FileExtension = file.FileExtension,
    //                    IsSmartRxRequested = file.IsSmartRxRequested,
    //                    HasRelative = file.HasRelative,
    //                    PatientRelativesId = file.PatientRelativesId,
    //                    IsSmarted = file.IsSmarted,
    //                    IsWaiting = file.IsWaiting,
    //                    CreatedById = file.CreatedById,
    //                    Tag1 = file.Tag1,
    //                    Tag2 = file.Tag2,
    //                    Tag3 = file.Tag3,
    //                    Tag4 = file.Tag4,
    //                    Tag5 = file.Tag5
    //                }).ToList();
    //            childNode.AddRange(folderNodeFiles);
    //        }
    //        foreach (var item in childrenFolders)
    //        {
    //            var combinedChildren = BuildChildren(root.FolderId, allFolders, allPrescriptions, paging, visited);
    //            var total = combinedChildren.Count;
    //            var page = combinedChildren
    //                .Skip((paging.PageNumber - 1) * paging.PageSize)
    //                .Take(paging.PageSize)
    //                .ToList();

    //            root.Children = new PaginatedResult<FolderNodeDTO>(page, total, paging.PageNumber, paging.PageSize, paging.SortBy, paging.SortDirection, null);

    //        }
    //        //var child = BuildChildren(parentFolderId, allFolders, allPrescriptions, paging, visited);


    //        //var combined = childrenFolders.Concat(childNode);


    //        //var sortBy = (paging.SortBy ?? "createddate").ToLower();
    //        //var sortDir = (paging.SortDirection ?? "asc").ToLower();
    //        //IOrderedEnumerable<FolderNodeDTO> ordered = sortBy is "filename" or "name"
    //        //    ? (sortDir == "desc" ? combined.OrderByDescending(x => x.FolderOrFileName) : combined.OrderBy(x => x.FolderOrFileName))
    //        //    : (sortDir == "desc" ? combined.OrderByDescending(x => x.CreatedDate) : combined.OrderBy(x => x.CreatedDate));

    //        //var total = ordered.Count();
    //        //var page = ordered.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToList();

    //        //root.Children = new PaginatedResult<FolderNodeDTO>(page, total, paging.PageNumber, paging.PageSize, paging.SortBy, paging.SortDirection, null);


    //        //var childFolderNodes = new List<FolderNodeDTO>();
    //        //foreach (var f in childrenFolders)
    //        //{
    //        //    var childNodeFinal = BuildFolderTree(f.FolderId, allFolders, allPrescriptions, paginatedPrescriptions, paging, false, new HashSet<long>(visited));
    //        //    childFolderNodes.Add(childNode);
    //        //}            
    //        return root;
    //    }

    //    private List<FolderNodeDTO> BuildChildren(
    //                                    long? parentFolderId,
    //                                    List<FolderNodeDTO> allFolders,
    //                                    List<PrescriptionDTO> allPrescriptions,
    //                                    PagingSortingParams paging,
    //                                    HashSet<long> visited)
    //    {
    //        var childrenFolders = new List<FolderNodeDTO>();

    //        // 1) Gather direct child folders of current parent
    //        var directFolders = parentFolderId is null
    //            ? allFolders.Where(f => f.ParentFolderId == null && f.FolderHeirarchy == 0).ToList()
    //            : allFolders.Where(f => f.ParentFolderId == parentFolderId && f.FolderId != parentFolderId).ToList();

    //        // 2) Recurse each folder and add to childrenFolders
    //        foreach (var f in directFolders)
    //        {
    //            if (visited.Contains(f.FolderId)) continue;
    //            var branchVisited = new HashSet<long>(visited) { f.FolderId };

    //            var node = BuildFolderTree(
    //                f.FolderId,
    //                allFolders,
    //                allPrescriptions,
    //                paging: paging,
    //                visited: branchVisited
    //            );

    //            childrenFolders.Add(node);
    //        }

    //        // 3) Add files scoped to current parent
    //        IEnumerable<PrescriptionDTO> filesQuery = allPrescriptions.AsEnumerable();
    //        filesQuery = parentFolderId is null
    //            ? filesQuery.Where(x => x.FolderHeirarchy == 0 && x.ParentFolderId == null)
    //            : filesQuery.Where(x => x.ParentFolderId == parentFolderId);

    //        foreach (var file in filesQuery)
    //        {
    //            childrenFolders.Add(new FolderNodeDTO
    //            {
    //                IsFolder = false,
    //                FolderId = file.FolderId,
    //                FileId = file.FileId,
    //                FolderOrFileName = file.FileName,
    //                CreatedDate = file.CreatedDate,
    //                CreatedDateStr = file.CreatedDateStr,
    //                ParentFolderId = file.ParentFolderId,
    //                UserId = file.UserId
    //            });
    //        }

    //        // 4) Sort + page the combined list (optional here; you can do it at the caller)
    //        var sortBy = (paging.SortBy ?? "createddate").ToLower();
    //        var sortDir = (paging.SortDirection ?? "asc").ToLower();

    //        IOrderedEnumerable<FolderNodeDTO> ordered = sortBy is "filename" or "name"
    //            ? (sortDir == "desc" ? childrenFolders.OrderByDescending(x => x.FolderOrFileName) : childrenFolders.OrderBy(x => x.FolderOrFileName))
    //            : (sortDir == "desc" ? childrenFolders.OrderByDescending(x => x.CreatedDate) : childrenFolders.OrderBy(x => x.CreatedDate));

    //        return ordered.ToList();
    //    }
    //}
}