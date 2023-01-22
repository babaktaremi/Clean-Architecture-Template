using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Queries.GetAllRolesQuery;

public record GetAllRolesQueryModel():IRequest<OperationResult<List<GetAllRolesQueryResult>>>;