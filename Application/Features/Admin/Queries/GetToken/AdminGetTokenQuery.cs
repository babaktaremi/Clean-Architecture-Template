using Application.Models.Common;
using Application.Models.Jwt;
using MediatR;

namespace Application.Features.Admin.Queries.GetToken
{
   public class AdminGetTokenQuery:IRequest<OperationResult<AccessToken>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
