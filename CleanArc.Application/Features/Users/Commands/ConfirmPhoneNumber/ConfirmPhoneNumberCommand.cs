using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using MediatR;

namespace CleanArc.Application.Features.Users.Commands.ConfirmPhoneNumber;

public record ConfirmPhoneNumberCommand(string UserKey, string Code) : IRequest<OperationResult<AccessToken>>;
