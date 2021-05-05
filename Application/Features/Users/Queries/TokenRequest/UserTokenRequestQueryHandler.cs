using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Contracts.Identity;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Users.Queries.TokenRequest
{
   public class UserTokenRequestQueryHandler:IRequestHandler<UserTokenRequestQuery,OperationResult<UserTokenRequestQueryResult>>
   {
       private readonly IAppUserManager _userManager;
       private readonly IMediator _mediator;

       public UserTokenRequestQueryHandler(IAppUserManager userManager, IMediator mediator)
       {
           _userManager = userManager;
           _mediator = mediator;
       }


        public async Task<OperationResult<UserTokenRequestQueryResult>> Handle(UserTokenRequestQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByPhoneNumber(request.UserPhoneNumber);

            if(user is null)
                return OperationResult<UserTokenRequestQueryResult>.NotFoundResult("کاربر یافت نشد");

            var code = await _userManager.GenerateOtpCode(user);

            //TODO Send Code Via Sms Provider

            return OperationResult<UserTokenRequestQueryResult>.SuccessResult(new UserTokenRequestQueryResult{UserKey = user.GeneratedCode});
        }
    }
}
