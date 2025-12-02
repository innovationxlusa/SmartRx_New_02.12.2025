using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PatientFolders
{
    public class CheckEmptyOrNewUserQuery : IRequest<bool>
    {
        public long UserId { get; set; }
    }

    public class CheckEmptyOrNewUserQueryHandler : IRequestHandler<CheckEmptyOrNewUserQuery, bool>
    {
        private readonly IUserWiseFolderRepository _userFolderRepository;

        public CheckEmptyOrNewUserQueryHandler(IUserWiseFolderRepository userFolderRepository)
        {
            _userFolderRepository = userFolderRepository;
        }
        public async Task<bool> Handle(CheckEmptyOrNewUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isExistFile = await _userFolderRepository.IsUploadedAnyFileForThisUser(request.UserId);

                await Task.CompletedTask;
                return isExistFile;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}