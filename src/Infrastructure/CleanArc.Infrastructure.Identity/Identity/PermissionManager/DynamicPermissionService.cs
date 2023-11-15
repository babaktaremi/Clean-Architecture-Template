using System.Security.Claims;

namespace CleanArc.Infrastructure.Identity.Identity.PermissionManager;

public class DynamicPermissionService : IDynamicPermissionService
{
    public bool CanAccess(ClaimsPrincipal user, string area, string controller, string action)
    {
        if (user.IsInRole("admin"))
        {
            return true;
        }


        var key = $"{area}:{controller}:";

        var userClaims = user.FindAll(ConstantPolicies.DynamicPermission);

        return userClaims.Any(item => item.Value.Equals(key, StringComparison.OrdinalIgnoreCase));
    }

  
}