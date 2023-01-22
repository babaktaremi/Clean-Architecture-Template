using CleanArc.Application.Models.Identity;
using CleanArc.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace CleanArc.Application.Contracts.Identity;

public interface IRoleManagerService
{
    Task<List<GetRolesDto>> GetRolesAsync();
    Task<IdentityResult> CreateRoleAsync(CreateRoleDto model);
    Task<bool> DeleteRoleAsync(int roleId);
    Task<List<ActionDescriptionDto>> GetPermissionActionsAsync();
    Task<RolePermissionDto> GetRolePermissionsAsync(int roleId);
    Task<bool> ChangeRolePermissionsAsync(EditRolePermissionsDto model);
    Task<Role> GetRoleByIdAsync(int roleId);
}