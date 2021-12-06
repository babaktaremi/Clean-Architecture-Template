using System.Security.Claims;
using Domain.Entities.User;
using Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Identity
{
    public class AppUserClaimsPrincipleFactory:UserClaimsPrincipalFactory<User,Role>
    {
        public AppUserClaimsPrincipleFactory(AppUserManager userManager, AppRoleManager roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

       
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString(),ClaimValueTypes.Integer));
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Email,user?.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));
           // claimsIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone,user.PhoneNumber));
           claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData,user.GeneratedCode));

            foreach (var roles in userRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role,roles));
            }

            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Role,RoleManager.GetRoleNameAsync(user.Roles)));
            

            return claimsIdentity;
        }
    }
}
