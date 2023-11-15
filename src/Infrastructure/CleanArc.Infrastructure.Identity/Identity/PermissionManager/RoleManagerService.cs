using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Identity;
using CleanArc.Domain.Entities.User;
using CleanArc.Infrastructure.Identity.Identity.Manager;
using CleanArc.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArc.Infrastructure.Identity.Identity.PermissionManager;

internal class RoleManagerService : IRoleManagerService
{
    private readonly AppRoleManager _roleManger;
    private readonly IMapper _mapper;
    private readonly IActionDescriptorCollectionProvider _actionDescriptor;
    private readonly EndpointDataSource _endpointDataSource;
    private readonly AppUserManager _userManager;
    private readonly ILogger<RoleManagerService> _logger;
    private readonly ApplicationDbContext _db;

    public RoleManagerService(AppRoleManager roleManger, IMapper mapper, IActionDescriptorCollectionProvider actionDescriptor, AppUserManager userManager, ILogger<RoleManagerService> logger, ApplicationDbContext db, EndpointDataSource endpointDataSource)
    {
        _roleManger = roleManger;
        _mapper = mapper;
        _actionDescriptor = actionDescriptor;
        _userManager = userManager;
        _logger = logger;
        _db = db;
        _endpointDataSource = endpointDataSource;
    }

    public async Task<List<GetRolesDto>> GetRolesAsync()
    {
        var result = await _roleManger.Roles.Where(c => !c.Name.Equals("admin")).Select(r => _mapper.Map<Role, GetRolesDto>(r)).ToListAsync();
        return result;
    }

    public async Task<IdentityResult> CreateRoleAsync(CreateRoleDto model)
    {
        var role = new Role
        {
            Name = model.RoleName,
        };

        var result = await _roleManger.CreateAsync(role);

        return result;
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        var role = await _roleManger.Roles.Include(r => r.Claims)
            .Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
            return false;

        var users = await _userManager.GetUsersInRoleAsync(role.Name);

        foreach (var user in users)
        {
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        _db.RemoveRange(role.Claims);
        _db.RemoveRange(role.Users);
        _db.Remove(role);
        await _db.SaveChangesAsync();

        return true;
    }

    public Task<List<ActionDescriptionDto>> GetPermissionActionsAsync()
    {
       
        var actions = new List<ActionDescriptionDto>();

        var actionDescriptors = _actionDescriptor.ActionDescriptors.Items;
        string controllerName = "";
        foreach (var actionDescriptor in actionDescriptors)
        {
            try
            {
                var descriptor = (ControllerActionDescriptor)actionDescriptor;

                var hasPermission = descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>()?
                    .Policy == ConstantPolicies.DynamicPermission; //||
                // descriptor.MethodInfo.GetCustomAttribute<AuthorizeAttribute>()?
                //   .Policy == ConstantPolicies.DynamicPermission;



                if (hasPermission && (controllerName != descriptor.ControllerName))
                {
                    actions.Add(new ActionDescriptionDto
                    {
                        // ActionName = descriptor.ActionName,
                        ControllerName = descriptor.ControllerName,
                        ActionDisplayName = descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.Name,
                        AreaName = descriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
                        ControllerDisplayName = descriptor.ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>()
                            ?.Name,
                        ControllerDescription = descriptor.ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>()
                            ?.Description
                    });
                    controllerName = descriptor.ControllerName;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        return Task.FromResult(actions);
    }

    public async Task<RolePermissionDto> GetRolePermissionsAsync(int roleId)
    {
        var role = await _roleManger.Roles
            .Include(x => x.Claims)
            .SingleOrDefaultAsync(x => x.Id == roleId);

        if (role == null)
            return null;

        var dynamicActions = await this.GetPermissionActionsAsync();
        return new RolePermissionDto
        {
            Role = role,
            Actions = dynamicActions
        };
    }

    public async Task<bool> ChangeRolePermissionsAsync(EditRolePermissionsDto model)
    {
        var role = await _roleManger.Roles
            .Include(x => x.Claims)
            .SingleOrDefaultAsync(x => x.Id == model.RoleId);

        if (role == null)
        {
            return false;
        }

        var selectedPermissions = model.Permissions;

        var roleClaims = role.Claims
            .Where(x => x.ClaimType == ConstantPolicies.DynamicPermission)
            .Select(x => x.ClaimValue)
            .ToList();


        // add new permissions 
        var newPermissions = selectedPermissions.Except(roleClaims).ToList();
        foreach (var permission in newPermissions)
        {
            role.Claims.Add(new RoleClaim
            {
                ClaimType = ConstantPolicies.DynamicPermission,
                ClaimValue = permission,
                CreatedClaim = DateTime.Now,
                RoleId = role.Id
            });
        }

        // remove deleted permissions
        var removedPermissions = roleClaims.Except(selectedPermissions).ToList();
        foreach (var permission in removedPermissions)
        {
            var roleClaim = role.Claims
                .SingleOrDefault(x =>
                    x.ClaimType == ConstantPolicies.DynamicPermission &&
                    x.ClaimValue == permission);

            if (roleClaim != null)
            {
                _db.RemoveRange(roleClaim);
            }
        }
        await _db.SaveChangesAsync();

        var result = await _roleManger.UpdateAsync(role);

        if (result.Succeeded)
        {
            var users = await _userManager.GetUsersInRoleAsync(role.Name);

            foreach (var user in users)
            {
                await _userManager.UpdateSecurityStampAsync(user);
            }

            return true;
        }

        return false;
    }

    public async Task<Role> GetRoleByIdAsync(int roleId)
    {
        return await _roleManger.FindByIdAsync(roleId.ToString());
    }
}