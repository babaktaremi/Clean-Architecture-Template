using Application.Models.Common;
using Application.Models.Jwt;
using MediatR;

namespace Application.Features.Users.Queries.GenerateUserToken.Model
{
  public class GenerateUserTokenQuery:IRequest<OperationResult<AccessToken>>
    {
        public string UserKey { get; set; }
        public string Code { get; set; }
    }
}
