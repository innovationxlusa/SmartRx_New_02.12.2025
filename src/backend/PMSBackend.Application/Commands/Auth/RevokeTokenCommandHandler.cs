using MediatR;
using PMSBackend.Application.CommonServices.Interfaces;

namespace PMSBackend.Application.Commands.Auth
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly ITokenGenerator _tokenGenerator;

        public RevokeTokenCommandHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenGenerator.RevokeTokenAsync(request.RefreshToken, cancellationToken);
        }
    }
}

