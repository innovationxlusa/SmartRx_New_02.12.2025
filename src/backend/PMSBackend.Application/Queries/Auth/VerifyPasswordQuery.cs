using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.Auth
{
    public class VerifyPasswordQuery : IRequest<bool>
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public int AuthType { get; set; }


    }
    public class VerifyPasswordHandler : IRequestHandler<VerifyPasswordQuery, bool>
    {
        private readonly IUserRepository _userRepository;



        public VerifyPasswordHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(VerifyPasswordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // code for verify otp first then below code
                var user = await _userRepository.GetDetailsByIdAsync(request.UserId);
                if (user == null)
                {
                    var isPasswordMatched = await _userRepository.VerifyPassword(user, request.Password);
                    if (isPasswordMatched != null && isPasswordMatched)
                    {
                        return true;
                    }
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