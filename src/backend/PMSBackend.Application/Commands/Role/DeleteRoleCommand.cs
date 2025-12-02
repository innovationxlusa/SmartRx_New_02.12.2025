using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Role
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public long RoleId { get; set; }
    }
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _roleRepository.DeleteAsync(request.RoleId);
                await Task.CompletedTask;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
