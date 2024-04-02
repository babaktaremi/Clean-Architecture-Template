using CleanArc.Application.Contracts.Persistence;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

public class DeleteUserOrdersCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserOrdersCommand,OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(DeleteUserOrdersCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.OrderRepository.DeleteUserOrdersAsync(request.UserId);

        return OperationResult<bool>.SuccessResult(true);
    }
}