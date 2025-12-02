using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PatientFolders
{
    public class GetAllFolderListQuery : IRequest<IEnumerable<UserWiseFolderDTO>>
    {
        public long UserId { get; set; }
    }

    public class GetAllFolderListQueryHandler : IRequestHandler<GetAllFolderListQuery, IEnumerable<UserWiseFolderDTO>>
    {
        private readonly IUserWiseFolderRepository _userFolderRepository;

        public GetAllFolderListQueryHandler(IUserWiseFolderRepository userFolderRepository)
        {
            _userFolderRepository = userFolderRepository;
        }
        public async Task<IEnumerable<UserWiseFolderDTO>> Handle(GetAllFolderListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var folderList = await _userFolderRepository.GetAllAsync(request.UserId);
                var dtoFolders = folderList.Select(p => new UserWiseFolderDTO
                {
                    Id = p.Id,
                    FolderName = p.FolderName,
                    ParentFolderId = p.ParentFolderId,
                    PatientId = p.PatientId,
                    FolderHierarchy = p.FolderHierarchy,
                    CreatedById = p.CreatedById,
                    CreatedDate = p.CreatedDate,
                    CreatedDateStr = Convert.ToDateTime(p.CreatedDate).ToString("dd-MM-yyyy"),
                    Description = p.Description,
                    UserId = p.UserId
                });
                await Task.CompletedTask;
                return dtoFolders;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}