using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Queries.GetAuthorizableRoutesQuery;

public record GetAuthorizableRoutesQueryModel():IRequest<OperationResult<List<GetAuthorizableRouteQueryResult>>>;