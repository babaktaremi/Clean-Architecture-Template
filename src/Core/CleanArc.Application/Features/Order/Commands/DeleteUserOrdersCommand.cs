using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

public record DeleteUserOrdersCommand(int UserId):IRequest<OperationResult<bool>>;