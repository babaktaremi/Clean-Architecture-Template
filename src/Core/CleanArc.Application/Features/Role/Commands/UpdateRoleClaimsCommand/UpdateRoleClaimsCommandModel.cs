
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Commands.UpdateRoleClaimsCommand;

public record UpdateRoleClaimsCommandModel( int RoleId, List<string> RoleClaimValue):IRequest<OperationResult<bool>>;