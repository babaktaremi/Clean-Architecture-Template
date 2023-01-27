using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CleanArc.Application.Features.Users.Queries.TokenRequest;

public class UserTokenRequestQueryHandler:IRequestHandler<UserTokenRequestQuery,OperationResult<UserTokenRequestQueryResult>>
{
    private readonly IAppUserManager _userManager;
    private readonly IMediator _mediator;
    private readonly ILogger<UserTokenRequestQueryHandler> _logger;

    public UserTokenRequestQueryHandler(IAppUserManager userManager, IMediator mediator, ILogger<UserTokenRequestQueryHandler> logger)
    {
        _userManager = userManager;
        _mediator = mediator;
        _logger = logger;
    }


    public async ValueTask<OperationResult<UserTokenRequestQueryResult>> Handle(UserTokenRequestQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserByPhoneNumber(request.UserPhoneNumber);

        if(user is null)
            return OperationResult<UserTokenRequestQueryResult>.NotFoundResult("User Not found");

        var code = user.PhoneNumberConfirmed? await _userManager.GenerateOtpCode(user) : await _userManager.GeneratePhoneNumberConfirmationToken(user,user.PhoneNumber);

        _logger.LogWarning($"Generated Code for user Id {user.Id} is {code}");

        //TODO Send Code Via Sms Provider

        return OperationResult<UserTokenRequestQueryResult>.SuccessResult(new UserTokenRequestQueryResult{UserKey = user.GeneratedCode});
    }
}