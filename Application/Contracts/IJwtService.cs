using System.Security.Claims;
using Application.Models.Jwt;
using Domain.Entities.User;

namespace Application.Contracts
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<AccessToken> GenerateByPhoneNumberAsync(string phoneNumber);
        Task<AccessToken> RefreshToken(string refreshTokenId);
    }
}