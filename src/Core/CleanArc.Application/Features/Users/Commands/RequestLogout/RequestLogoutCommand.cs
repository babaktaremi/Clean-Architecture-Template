using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RequestLogout;

public record RequestLogoutCommand(int UserId):IRequest<OperationResult<bool>>;