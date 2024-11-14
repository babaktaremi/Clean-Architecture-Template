using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

internal class AddOrderCommandHandler(IUnitOfWork unitOfWork, IAppUserManager userManager)
    : IRequestHandler<AddOrderCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserByIdAsync(request.UserId);

        if(user==null)
            return OperationResult<bool>.FailureResult("User Not Found");

        await unitOfWork.OrderRepository.AddOrderAsync(new Domain.Entities.Order.Order()
            { UserId = user.Id, OrderName = request.OrderName });

        await unitOfWork.CommitAsync();

        return OperationResult<bool>.SuccessResult(true);
    }
}