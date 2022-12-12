using CleanArc.Application.Models.Common;
using MediatR;

namespace CleanArc.Application.Features.Users.Queries.TokenRequest;

public class UserTokenRequestQuery:IRequest<OperationResult<UserTokenRequestQueryResult>>
{
    public string UserPhoneNumber { get; set; }
}