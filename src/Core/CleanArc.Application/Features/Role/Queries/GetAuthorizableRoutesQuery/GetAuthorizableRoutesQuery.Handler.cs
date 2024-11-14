using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Queries.GetAuthorizableRoutesQuery
{
    internal class GetAuthorizableRoutesQueryHandler(IRoleManagerService roleManagerService)
        : IRequestHandler<GetAuthorizableRoutesQuery, OperationResult<List<GetAuthorizableRoutesQueryResponse>>>
    {
        public async ValueTask<OperationResult<List<GetAuthorizableRoutesQueryResponse>>> Handle(GetAuthorizableRoutesQuery request, CancellationToken cancellationToken)
        {
            var authRoutes = await roleManagerService.GetPermissionActionsAsync();

            if(!authRoutes.Any())
                return OperationResult<List<GetAuthorizableRoutesQueryResponse>>.NotFoundResult("No Special auth route found");

            var result = authRoutes.Select(c =>
                    new GetAuthorizableRoutesQueryResponse(c.Key, c.AreaName, c.ControllerName, c.ActionName,c.ControllerDescription))
                .ToList();

            return OperationResult<List<GetAuthorizableRoutesQueryResponse>>.SuccessResult(result);
        }
    }
}
