using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Queries.GetAuthorizableRoutesQuery
{
    internal class GetAuthorizableRoutesQueryHandler:IRequestHandler<GetAuthorizableRoutesQueryModel,OperationResult<List<GetAuthorizableRouteQueryResult>>>
    {
        private readonly IRoleManagerService _roleManagerService;

        public GetAuthorizableRoutesQueryHandler(IRoleManagerService roleManagerService)
        {
            _roleManagerService = roleManagerService;
        }

        public async ValueTask<OperationResult<List<GetAuthorizableRouteQueryResult>>> Handle(GetAuthorizableRoutesQueryModel request, CancellationToken cancellationToken)
        {
            var authRoutes = await _roleManagerService.GetPermissionActionsAsync();

            if(!authRoutes.Any())
                return OperationResult<List<GetAuthorizableRouteQueryResult>>.NotFoundResult("No Special auth route found");

            var result = authRoutes.Select(c =>
                    new GetAuthorizableRouteQueryResult(c.Key, c.AreaName, c.ControllerName, c.ActionName,c.ControllerDescription))
                .ToList();

            return OperationResult<List<GetAuthorizableRouteQueryResult>>.SuccessResult(result);
        }
    }
}
