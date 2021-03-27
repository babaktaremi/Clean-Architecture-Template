using Application.Models.Common;
using MediatR;

namespace Application.Features.Users.Queries.TokenRequest
{
   public class UserTokenRequestQuery:IRequest<OperationResult<UserTokenRequestQueryResult>>
    {
        public string UserPhoneNumber { get; set; }
    }
}
