using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Admin.Queries.GetToken;

public record AdminGetTokenQuery(string UserName,string Password):IRequest<OperationResult<AccessToken>>;