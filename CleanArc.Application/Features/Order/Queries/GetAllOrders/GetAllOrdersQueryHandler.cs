using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Queries.GetAllOrders
{
    internal class GetAllOrdersQueryHandler:IRequestHandler<GetAllOrdersQuery,OperationResult<List<GetAllOrdersQueryResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<OperationResult<List<GetAllOrdersQueryResult>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllOrdersWithRelatedUserAsync();

            var result = orders.Select(c => new GetAllOrdersQueryResult(c.Id, c.OrderName, c.UserId, c.User.UserName)).ToList();

            return OperationResult<List<GetAllOrdersQueryResult>>.SuccessResult(result);
        }
    }
}
