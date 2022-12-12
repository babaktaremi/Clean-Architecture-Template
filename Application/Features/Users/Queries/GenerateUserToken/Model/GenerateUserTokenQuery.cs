using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using MediatR;

namespace CleanArc.Application.Features.Users.Queries.GenerateUserToken.Model;

public class GenerateUserTokenQuery:IRequest<OperationResult<AccessToken>>
{
    public string UserKey { get; set; }
    public string Code { get; set; }
}