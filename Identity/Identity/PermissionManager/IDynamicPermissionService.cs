using System.Security.Claims;

namespace Identity.Identity.PermissionManager
{
    public interface IDynamicPermissionService
    {
        bool CanAccess(ClaimsPrincipal user, string area, string controller, string action);
    }
}   