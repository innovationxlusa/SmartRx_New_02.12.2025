using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.Role
{
    public class GetRoleByRoleNameQuery : IRequest<RoleResponseDTO>
    {
        public string RoleName { get; set; }
    }

    public class GetRoleQueryByRoleNameHandler : IRequestHandler<GetRoleByRoleNameQuery, RoleResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleQueryByRoleNameHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleResponseDTO> Handle(GetRoleByRoleNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleRepository.GetDetailsByRoleNameAsync(request.RoleName);
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