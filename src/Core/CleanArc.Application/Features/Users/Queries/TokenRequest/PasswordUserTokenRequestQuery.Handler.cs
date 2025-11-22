using CleanArc.Application.Contracts;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Users.Queries.TokenRequest;

public class PasswordUserTokenRequestQueryResult
(IAppUserManager userManager,IJwtService jwtService)
:IRequestHandler<PasswordUserTokenRequestQuery,OperationResult<AccessToken>>
{
    public async ValueTask<OperationResult<AccessToken>> Handle(PasswordUserTokenRequestQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetByUserName(request.UserName);
        
        if(user is null)
            return OperationResult<AccessToken>.NotFoundResult("User not found");
        
        if(!await userManager.IsPasswordValidAsync(user,request.Password))
            return OperationResult<AccessToken>.NotFoundResult("User not found");
        
        var token = await jwtService.GenerateAsync(user);
        
        return OperationResult<AccessToken>.SuccessResult(token);
    }
}