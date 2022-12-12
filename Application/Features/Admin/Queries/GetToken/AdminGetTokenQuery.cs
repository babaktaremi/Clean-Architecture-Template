using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using MediatR;

namespace CleanArc.Application.Features.Admin.Queries.GetToken;

public class AdminGetTokenQuery:IRequest<OperationResult<AccessToken>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}