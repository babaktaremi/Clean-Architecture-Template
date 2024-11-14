using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RequestLogout
{
    internal class RequestLogoutCommandHandler(IAppUserManager userManager)
        : IRequestHandler<RequestLogoutCommand, OperationResult<bool>>
    {
        public async ValueTask<OperationResult<bool>> Handle(RequestLogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserByIdAsync(request.UserId);

            if (user == null) 
                return OperationResult<bool>.FailureResult("User not found");

            await userManager.UpdateSecurityStampAsync(user);

            return OperationResult<bool>.SuccessResult(true);
        }
    }
}
