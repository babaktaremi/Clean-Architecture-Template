using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Queries.GetAllRolesQuery
{
    internal class GetAllRolesQueryHandler:IRequestHandler<GetAllRolesQueryModel,OperationResult<List<GetAllRolesQueryResult>>>
    {
        private readonly IRoleManagerService _roleManagerService;

        public GetAllRolesQueryHandler(IRoleManagerService roleManagerService)
        {
            _roleManagerService = roleManagerService;
        }

        public async ValueTask<OperationResult<List<GetAllRolesQueryResult>>> Handle(GetAllRolesQueryModel request, CancellationToken cancellationToken)
        {
            var roles = await _roleManagerService.GetRolesAsync();

            if(!roles.Any())
                return OperationResult<List<GetAllRolesQueryResult>>.NotFoundResult("No Roles Found");

            var result = roles.Select(c => new GetAllRolesQueryResult(int.Parse(c.Id), c.Name)).ToList();

            return OperationResult<List<GetAllRolesQueryResult>>.SuccessResult(result);
        }
    }
}
