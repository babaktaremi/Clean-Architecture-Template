using CleanArc.Application.Contracts;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Admin.Queries.GetToken;

public class AdminGetTokenQueryHandler:IRequestHandler<AdminGetTokenQuery,OperationResult<AdminGetTokenQueryResult>>
{
    private readonly IAppUserManager _userManager;
    private readonly IJwtService _jwtService;
    public AdminGetTokenQueryHandler(IAppUserManager userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async ValueTask<OperationResult<AdminGetTokenQueryResult>> Handle(AdminGetTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetByUserName(request.UserName);

        if(user is null)
            return OperationResult<AdminGetTokenQueryResult>.FailureResult("User not found");

        var isUserLockedOut = await _userManager.IsUserLockedOutAsync(user);

        if(isUserLockedOut)
            if (user.LockoutEnd != null)
                return OperationResult<AdminGetTokenQueryResult>.FailureResult(
                    $"User is locked out. Try in {(user.LockoutEnd-DateTimeOffset.Now).Value.Minutes} Minutes");

        var userRoles = await _userManager.GetRoleAsync(user);


       if(!userRoles.Any())
           return OperationResult<AdminGetTokenQueryResult>.FailureResult("This user does not have any role assigned");

       if(!await _userManager.IsPasswordValidAsync(user, request.Password))
              return OperationResult<AdminGetTokenQueryResult>.NotFoundResult("User not found");
       
        var token= await _jwtService.GenerateAsync(user);


        return OperationResult<AdminGetTokenQueryResult>.SuccessResult(new(token,userRoles));
    }
}