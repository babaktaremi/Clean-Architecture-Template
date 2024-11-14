using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Identity;
using Mediator;

namespace CleanArc.Application.Features.Role.Commands.UpdateRoleClaimsCommand
{
    internal class UpdateRoleClaimsCommandHandler(IRoleManagerService roleManagerService)
        : IRequestHandler<UpdateRoleClaimsCommand, OperationResult<bool>>
    {
        public async ValueTask<OperationResult<bool>> Handle(UpdateRoleClaimsCommand request, CancellationToken cancellationToken)
        {
            var updateRoleResult = await roleManagerService.ChangeRolePermissionsAsync(new EditRolePermissionsDto()
                { RoleId = request.RoleId, Permissions = request.RoleClaimValue });

            return updateRoleResult
                ? OperationResult<bool>.SuccessResult(true)
                : OperationResult<bool>.FailureResult("Could Not Update Claims for given Role");
        }
    }
}
