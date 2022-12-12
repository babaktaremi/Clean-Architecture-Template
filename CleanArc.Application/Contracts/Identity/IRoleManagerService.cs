using CleanArc.Application.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArc.Application.Contracts.Identity;

public interface IRoleManagerService
{
    Task<List<GetRolesDto>> GetRoles();
    Task<IdentityResult> CreateRole(CreateRoleDto model);
    Task<bool> DeleteRole(int roleId);
    Task<List<ActionDescriptionDto>> GetPermissionActions();
    Task<RolePermissionDto> GetRolePermissions(int roleId);
    Task<bool> ChangeRolePermissions(EditRolePermissionsDto model);
}