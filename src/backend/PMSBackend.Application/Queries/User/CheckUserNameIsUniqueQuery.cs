using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.User
{
    public class CheckUserNameIsUniqueQuery : IRequest<bool>
    {
        public string UserName { get; set; }
    }
    public class CheckUserNameIsUniqueQueryHandler : IRequestHandler<CheckUserNameIsUniqueQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public CheckUserNameIsUniqueQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CheckUserNameIsUniqueQuery request, CancellationToken cancellationToken)
        {
            var isUserUnique = await _userRepository.IsUniqueUserName(request.UserName);

            await Task.CompletedTask;
            return isUserUnique;
        }
    }
}


