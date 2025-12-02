using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.User
{
    public class GetUserDetailsByUserNameQuery : IRequest<UserDetailsResponseDTO>
    {
        public string UserName { get; set; }
    }

    public class GetUserDetailsByUserNameQueryHandler : IRequestHandler<GetUserDetailsByUserNameQuery, UserDetailsResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public GetUserDetailsByUserNameQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<UserDetailsResponseDTO> Handle(GetUserDetailsByUserNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userDetailsWithRole = new UserDetailsResponseDTO();
                var user = await _userRepository.GetUserDetailsByUserNameAsync(request.UserName);
                if (user is not null)
                {
                    userDetailsWithRole.Id = user.Id;
                    userDetailsWithRole.Email = user.Email;
                    userDetailsWithRole.UserName = user.UserName;
                    userDetailsWithRole.FirstName = user.FirstName;
                    userDetailsWithRole.LastName = user.LastName;
                    userDetailsWithRole.Password = user.Password;
                    userDetailsWithRole.Roles = (IList<SmartRxUserRoleEntity>)_userRoleRepository.GetUserRolesAsync(user.Id);
                }
                await Task.CompletedTask;
                return userDetailsWithRole;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
