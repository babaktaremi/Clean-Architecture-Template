using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Role.Commands.AddRoleCommand;

public record AddRoleCommandModel(string RoleName):IRequest<OperationResult<bool>>;