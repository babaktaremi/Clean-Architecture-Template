using System.Diagnostics;
using Application.Contracts.Identity;
using Application.Models.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Queries.TokenRequest
{
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


        public async Task<OperationResult<UserTokenRequestQueryResult>> Handle(UserTokenRequestQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByPhoneNumber(request.UserPhoneNumber);

            if(user is null)
                return OperationResult<UserTokenRequestQueryResult>.NotFoundResult("کاربر یافت نشد");

            var code = await _userManager.GenerateOtpCode(user);

            _logger.LogWarning($"Generated Code for user Id {user.Id} is {code}");

            //TODO Send Code Via Sms Provider

            return OperationResult<UserTokenRequestQueryResult>.SuccessResult(new UserTokenRequestQueryResult{UserKey = user.GeneratedCode});
        }
    }
}
