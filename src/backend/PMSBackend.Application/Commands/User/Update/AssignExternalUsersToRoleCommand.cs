using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.User.Update
{
    public class AssignExternalUsersToRoleCommand : IRequest<long>
    {
        public string UserName { get; set; }
    }
    public class AssignExternalUsersToRoleCommandHandler : IRequestHandler<AssignExternalUsersToRoleCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AssignExternalUsersToRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<long> Handle(AssignExternalUsersToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserDetailsByUserNameAsync(request.UserName);
                if (user is not null)
                {
                    var isUpdated = await _userRoleRepository.UpdateExternalUsersRole(user.Id);
                    if (isUpdated) return user.Id;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
