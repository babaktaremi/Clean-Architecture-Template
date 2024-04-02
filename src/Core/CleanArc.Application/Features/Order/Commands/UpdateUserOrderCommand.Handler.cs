using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

public class UpdateUserOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserOrderCommand,OperationResult<bool>>
{


    public async ValueTask<OperationResult<bool>> Handle(UpdateUserOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetUserOrderByIdAndUserIdAsync(request.UserId, request.OrderId,
            true);

        if(order is null)
            return OperationResult<bool>.NotFoundResult("Specified Order not found");

        order.OrderName=request.OrderName;

        await unitOfWork.CommitAsync();

        return OperationResult<bool>.SuccessResult(true);
    }
}