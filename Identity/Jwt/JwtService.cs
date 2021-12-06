using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts;
using Application.Contracts.Persistence;
using Application.Models.Jwt;
using Domain.Entities.User;
using Identity.Identity.Dtos;
using Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IdentitySettings _siteSetting;
        private readonly AppUserManager _userManager;
        private IUserClaimsPrincipalFactory<User> _claimsPrincipal;

        private readonly IUnitOfWork _unitOfWork;
        //private readonly AppUserClaimsPrincipleFactory claimsPrincipleFactory;

        public JwtService(IOptions<IdentitySettings> siteSetting, AppUserManager userManager, IUserClaimsPrincipalFactory<User> claimsPrincipal, IUnitOfWork unitOfWork)
        {
            _siteSetting = siteSetting.Value;
            _userManager = userManager;
            _claimsPrincipal = claimsPrincipal;
            _unitOfWork = unitOfWork;
        }
        public async Task<AccessToken> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);


            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.Issuer,
                Audience = _siteSetting.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddMinutes(_siteSetting.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);


            var refreshToken = await _unitOfWork.UserRefreshTokenRepository.CreateToken(user.Id);
            await _unitOfWork.CommitAsync();

            return new AccessToken(securityToken,refreshToken.ToString());
        }

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.SecretKey)),
                ValidateLifetime = false,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.Encryptkey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }

        public async Task<AccessToken> GenerateByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            var result = await this.GenerateAsync(user);
            return result;
        }

        public async Task<AccessToken> RefreshToken(string refreshTokenId)
        {
            var refreshToken = await _unitOfWork.UserRefreshTokenRepository.GetTokenWithInvalidation(Guid.Parse(refreshTokenId));
            
            if (refreshToken is null)
                return null;

            await _unitOfWork.CommitAsync();

            var user = await _unitOfWork.UserRefreshTokenRepository.GetUserByRefreshToken(Guid.Parse(refreshTokenId));

            if (user is null)
                return null;

            var result = await this.GenerateAsync(user);

            return result;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var result = await _claimsPrincipal.CreateAsync(user);
            return result.Claims;
        }
    }
}
