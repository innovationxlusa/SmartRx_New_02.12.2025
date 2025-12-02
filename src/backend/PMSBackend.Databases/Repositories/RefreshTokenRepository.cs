using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Databases.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PMSDbContext _context;
        private readonly ILogger<RefreshTokenRepository> _logger;

        public RefreshTokenRepository(PMSDbContext context, ILogger<RefreshTokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.RefreshTokens
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving refresh token by token value");
                throw;
            }
        }

        public async Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding refresh token for UserId: {UserId}", refreshToken.UserId);
                throw;
            }
        }

        public async Task UpdateAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.RefreshTokens.Update(refreshToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating refresh token with TokenId: {TokenId}", refreshToken.Id);
                throw;
            }
        }

        public async Task<bool> RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var tokenIds = await _context.RefreshTokens
                    .AsNoTracking()
                    .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                    .Select(rt => rt.Id)
                    .ToListAsync(cancellationToken);

                if (!tokenIds.Any())
                {
                    _logger.LogInformation("No active tokens found to revoke for UserId: {UserId}", userId);
                    return false;
                }

                var tokens = await _context.RefreshTokens
                    .Where(rt => tokenIds.Contains(rt.Id))
                    .ToListAsync(cancellationToken);

                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                }
                    
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Revoked {Count} tokens for UserId: {UserId}", tokens.Count, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while revoking all tokens for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<RefreshTokenEntity>> GetActiveTokensByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.RefreshTokens
                    .AsNoTracking()
                    .Where(rt => rt.UserId == userId && !rt.IsRevoked && !rt.IsUsed && rt.ExpiryDate > DateTime.UtcNow)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active tokens for UserId: {UserId}", userId);
                throw;
            }
        }
    }
}

