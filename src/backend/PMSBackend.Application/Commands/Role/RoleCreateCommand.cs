using MediatR;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Role
{
    public class RoleCreateCommand : IRequest<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class RoleCreateCommandHandler : IRequestHandler<RoleCreateCommand, long>
    {
        private readonly IRoleRepository _roleRepository;
        public RoleCreateCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<long> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //string[] roleNames = { "Super Admin", "Admin", "Entry User", "Approver", "Recommender", "External User" };



                //var roles = new List<SmartRxRole>()
                //{
                //    new SmartRxRole { Name = "Super Admin", IsSelfService = false, Description="Super Admin is the only one user in the system and all super access has in it and to recover the system only this user can be usable " },
                //    new SmartRxRole { Name = "Admin", IsSelfService=false, Description="Admin is to manipulated all general access and manage system" },
                //    new SmartRxRole { Name = "Entry User", IsSelfService = true, Description="An admin user who can enter all data into the system" },
                //    new SmartRxRole { Name = "Approver", IsSelfService = true },
                //    new SmartRxRole { Name = "Recommender", IsSelfService = true },
                //    new SmartRxRole { Name = "External User", IsSelfService = false, Description = "Only outside user are in this list" }
                //};
                //foreach (var role in roles)
                //{
                //    if (!await _roleRepository.IsExistsRole(role.Name))
                //    {
                //        await _roleRepository.AddAsync(role);
                //    }
                //}

                var newRole = new SmartRxRoleEntity();
                newRole.Name = request.Name;
                newRole.Description = request.Description;
                newRole.IsSelfService = false;
                var result = await _roleRepository.AddAsync(newRole);
                return result.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
