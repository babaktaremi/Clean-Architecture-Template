using Application.Models.Common;
using Application.Models.Jwt;
using MediatR;

namespace Application.Features.Users.Commands.ConfirmPhoneNumber
{
   public class ConfirmPhoneNumberCommand:IRequest<OperationResult<AccessToken>>
    {
        public string UserKey { get; set; }
        public string Code { get; set; }
    }
}
