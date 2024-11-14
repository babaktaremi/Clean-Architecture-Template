using AutoMapper;
using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Queries.GetAllOrders
{
    internal class GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllOrdersQuery, OperationResult<List<GetAllOrdersQueryResult>>>
    {
        public async ValueTask<OperationResult<List<GetAllOrdersQueryResult>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.OrderRepository.GetAllOrdersWithRelatedUserAsync();

            var result = orders.Select(mapper.Map<Domain.Entities.Order.Order,GetAllOrdersQueryResult>).ToList();

            return OperationResult<List<GetAllOrdersQueryResult>>.SuccessResult(result);
        }
    }
}
