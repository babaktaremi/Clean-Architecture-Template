using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

public record AddOrderCommand(int UserId, string OrderName) : IRequest<OperationResult<bool>>;