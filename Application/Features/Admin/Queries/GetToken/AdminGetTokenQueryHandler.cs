using Application.Contracts;
using Application.Contracts.Identity;
using Application.Models.Common;
using Application.Models.Jwt;
using MediatR;

namespace Application.Features.Admin.Queries.GetToken
{
   public class AdminGetTokenQueryHandler:IRequestHandler<AdminGetTokenQuery,OperationResult<AccessToken>>
   {
       private readonly IAppUserManager _userManager;
       private readonly IJwtService _jwtService;
       public AdminGetTokenQueryHandler(IAppUserManager userManager, IJwtService jwtService)
       {
           _userManager = userManager;
           _jwtService = jwtService;
       }

        public async Task<OperationResult<AccessToken>> Handle(AdminGetTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetByUserName(request.UserName);

            if(user is null)
                return OperationResult<AccessToken>.FailureResult("کاربر یافت نشد");

            var passwordValidator = await _userManager.AdminLogin(user, request.Password);

            if (passwordValidator.IsLockedOut)
                return OperationResult<AccessToken>.FailureResult("کاربر قفل شده است 5 دقیقه بعد تلاش نمایید");

            if (!passwordValidator.Succeeded)
                return OperationResult<AccessToken>.FailureResult("نام کاربری یا رمز عبور صحیح نیست");

           var token= await _jwtService.GenerateAsync(user);


            return OperationResult<AccessToken>.SuccessResult(token);
        }
    }
}
