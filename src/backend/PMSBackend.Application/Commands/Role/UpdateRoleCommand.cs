using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Role
{
    public class UpdateRoleCommand : IRequest<long?>
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; } = default!;
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, long?>
    {
        private readonly IRoleRepository _roleRepository;
        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<long?> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _roleRepository.GetDetailsByIdAsync(request.Id);
                if (entity is not null)
                {
                    if (!string.IsNullOrWhiteSpace(request.RoleName)) entity.Name = request.RoleName;
                    if (!string.IsNullOrWhiteSpace(request.RoleDescription)) entity.Description = request.RoleDescription;
                    await _roleRepository.UpdateAsync(entity);
                    await Task.CompletedTask;
                }
                return entity?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
