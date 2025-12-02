using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Data;

namespace PMSBackend.Application.Queries.Role
{
    public class GetRoleQuery : IRequest<IList<RoleResponseDTO>>
    {

    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, IList<RoleResponseDTO>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IList<RoleResponseDTO>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                var rolesDTO = roles.Where(r => !r.IsSelfService).Select(r => new RoleResponseDTO()
                {
                    Id = r.Id,
                    RoleName = r.Name,
                    RoleDescription = r.Description,
                }).ToList();
                await Task.CompletedTask;
                return rolesDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
