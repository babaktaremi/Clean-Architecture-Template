using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using MediatR;

namespace CleanArc.Application.Features.Users.Commands.ConfirmPhoneNumber;

public class ConfirmPhoneNumberCommand:IRequest<OperationResult<AccessToken>>
{
    public string UserKey { get; set; }
    public string Code { get; set; }
}