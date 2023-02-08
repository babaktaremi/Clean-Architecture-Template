using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RequestLogout;

public record RequestLogoutCommandModel(int UserId):IRequest<OperationResult<bool>>;