using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.Role
{
    public class GetRoleByIdQuery : IRequest<RoleResponseDTO>
    {
        public long RoleId { get; set; }
    }

    public class GetRoleQueryByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleQueryByIdHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleResponseDTO> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleRepository.GetDetailsByIdAsync(request.RoleId);
                await Task.CompletedTask;
                return new RoleResponseDTO() { Id = role.Id, RoleName = role.Name, RoleDescription = role.Description };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
