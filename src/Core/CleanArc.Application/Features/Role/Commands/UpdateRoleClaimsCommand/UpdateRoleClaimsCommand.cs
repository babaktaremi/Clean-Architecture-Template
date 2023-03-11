
using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Commands.UpdateRoleClaimsCommand;

public record UpdateRoleClaimsCommand( int RoleId, List<string> RoleClaimValue):IRequest<OperationResult<bool>>;