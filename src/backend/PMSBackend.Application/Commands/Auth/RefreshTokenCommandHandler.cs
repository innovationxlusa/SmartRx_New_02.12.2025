using MediatR;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.Commands.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthTokenResponseDTO?>
    {
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenCommandHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthTokenResponseDTO?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenGenerator.RefreshTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken);
        }
    }
}

