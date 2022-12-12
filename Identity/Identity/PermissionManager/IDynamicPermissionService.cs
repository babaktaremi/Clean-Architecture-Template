using System.Security.Claims;

namespace CleanArc.Infrastructure.Identity.Identity.PermissionManager;

public interface IDynamicPermissionService
{
    bool CanAccess(ClaimsPrincipal user, string area, string controller, string action);
}