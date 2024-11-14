using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Queries.GetUserOrders;

internal class GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserOrdersQueryModel, OperationResult<List<GetUsersQueryResultModel>>>
{
    public async ValueTask<OperationResult<List<GetUsersQueryResultModel>>> Handle(GetUserOrdersQueryModel request, CancellationToken cancellationToken)
    {
        var orders = await unitOfWork.OrderRepository.GetAllUserOrdersAsync(request.UserId);

        if(!orders.Any())
            return OperationResult<List<GetUsersQueryResultModel>>.NotFoundResult("You Don't Have Any Orders");

        var result = orders.Select(c => new GetUsersQueryResultModel(c.Id, c.OrderName));

        return OperationResult<List<GetUsersQueryResultModel>>.SuccessResult(result.ToList());
    }
}