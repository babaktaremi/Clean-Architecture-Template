using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RefreshUserTokenCommand;

public record RefreshUserTokenCommand(string RefreshToken):IRequest<OperationResult<AccessToken>>;