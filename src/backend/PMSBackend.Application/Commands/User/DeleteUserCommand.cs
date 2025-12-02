using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.User
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _userRepository.GetDetailsByIdAsync(request.Id);
                if (user is not null)
                {
                    await _userRoleRepository.DeleteUserRoleAsync(request.Id);
                    await _userRepository.DeleteAsync(request.Id);
                    await Task.CompletedTask;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
