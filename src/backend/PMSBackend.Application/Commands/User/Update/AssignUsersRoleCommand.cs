using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.User.Update
{
    public class AssignUsersRoleCommand : IRequest<long>
    {
        public string UserName { get; set; } = default!;
        public IList<long> Roles { get; set; } = default!;
    }
    public class AssignUsersRoleCommandHandler : IRequestHandler<AssignUsersRoleCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AssignUsersRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<long> Handle(AssignUsersRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserDetailsByUserNameAsync(request.UserName);
                if (user is not null)
                {
                    var isUpdated = await _userRoleRepository.UpdateUsersRole(user.Id, request.Roles);
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
