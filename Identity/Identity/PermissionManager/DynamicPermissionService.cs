using System.Security.Claims;

namespace Identity.Identity.PermissionManager
{
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

            foreach (var item in userClaims)
            {
                if (item.Value.Equals(key,StringComparison.OrdinalIgnoreCase))
                    return true;
                else
                {
                    continue;
                }
               
            }

            return false;
        }
    }
}