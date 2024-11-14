using CleanArc.Application.Contracts;
using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RefreshUserTokenCommand
{
    internal class RefreshUserTokenCommandHandler(IJwtService jwtService)
        : IRequestHandler<RefreshUserTokenCommand, OperationResult<AccessToken>>
    {
        public async ValueTask<OperationResult<AccessToken>> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
        {
            var newToken = await jwtService.RefreshToken(request.RefreshToken);

            if(newToken is null)
                return OperationResult<AccessToken>.FailureResult("Invalid refresh token");

            return OperationResult<AccessToken>.SuccessResult(newToken);
        }
    }
}
