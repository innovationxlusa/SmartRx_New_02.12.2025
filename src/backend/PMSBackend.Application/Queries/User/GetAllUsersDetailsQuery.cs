using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.User
{
    public class GetAllUsersDetailsQuery : IRequest<UsersDTO>
    {
        //public long Id { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public string? FullName { get; set; }
        //public string? MobileNo { get; set; } = string.Empty!;
        //public string? Email { get; set; } = string.Empty!;
        //public string? GoogleId { get; set; } = string.Empty!;
        //public string? FacebookId { get; set; } = string.Empty!;
        //public string? TwitterId { get; set; } = string.Empty!;
        //public string? FirstName { get; set; } = string.Empty!;
        //public string? LastName { get; set; } = string.Empty!;
        //public int? AuthMethod { get; set; }
        //public int? EmployeeId { get; set; }
        //public string? EmployeeCode { get; set; }
        //public int? Gender { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public int? Status { get; set; }
        //public List<long>? Roles { get; set; }

    }

    public class GetAllUsersDetailsQueryHandler : IRequestHandler<GetAllUsersDetailsQuery, UsersDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public GetAllUsersDetailsQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<UsersDTO> Handle(GetAllUsersDetailsQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new UsersDTO();
            var users = await _userRepository.GetAllAsync();
            if (users is null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "No User found!"
                };
                return responseResult;
            }

            var userDetails = users.Select(x => new UserDetailsResponseDTO()
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Password = x.Password!,
                Roles = x.UserRoles!.ToList()
            }).ToList();

            responseResult.Users = userDetails;
            responseResult.ApiResponseResult = null;
            responseResult.ConnectionString = await _userRepository.GetConnectionString();

            await Task.CompletedTask;
            return responseResult;
        }
    }
}
