using Application.Contracts.Persistence;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    internal class UserRefreshTokenRepository : BaseAsyncRepository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Guid> CreateToken(int userId)
        {
            var token = new UserRefreshToken { IsValid = true, UserId = userId };
            await base.AddAsync(token);
            return token.Id;
        }

        public async Task<UserRefreshToken> GetTokenWithInvalidation(Guid id)
        {
            var token = await base.Table.Where(t => t.IsValid && t.Id.Equals(id)).FirstOrDefaultAsync();

            if (token == null) return null;
            token.IsValid = false;
            await base.UpdateAsync(token);

            return token;
        }

        public async Task<User> GetUserByRefreshToken(Guid tokenId)
        {
            var user = await base.TableNoTracking.Include(t => t.User).Where(c => c.Id.Equals(tokenId))
                .Select(c => c.User).FirstOrDefaultAsync();

            return user;
        }

        public Task RemoveUserOldTokens(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
