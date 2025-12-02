using MediatR;
using PMSBackend.Application.CommonServices.Interfaces;

namespace PMSBackend.Application.Commands.Auth
{
    public class RevokeAllUserTokensCommandHandler : IRequestHandler<RevokeAllUserTokensCommand, bool>
    {
        private readonly ITokenGenerator _tokenGenerator;

        public RevokeAllUserTokensCommandHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> Handle(RevokeAllUserTokensCommand request, CancellationToken cancellationToken)
        {
            return await _tokenGenerator.RevokeAllUserTokensAsync(request.UserId, cancellationToken);
        }
    }
}

