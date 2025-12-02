using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.User
{
    public class GetUserDetailsQuery : IRequest<UserDetailsResponseDTO>
    {
        public long UserId { get; set; } = default!;
    }

    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public GetUserDetailsQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<UserDetailsResponseDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userDetailsWithRole = new UserDetailsResponseDTO();
                var user = await _userRepository.GetDetailsByIdAsync(request.UserId);
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
