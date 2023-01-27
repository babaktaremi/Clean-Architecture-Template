using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Users.Queries.GenerateUserToken.Model;

public record GenerateUserTokenQuery(string UserKey,string Code):IRequest<OperationResult<AccessToken>>;