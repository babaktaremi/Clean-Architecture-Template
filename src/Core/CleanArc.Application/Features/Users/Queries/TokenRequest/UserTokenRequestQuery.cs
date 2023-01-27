using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Users.Queries.TokenRequest;

public record UserTokenRequestQuery(string UserPhoneNumber):IRequest<OperationResult<UserTokenRequestQueryResult>>;