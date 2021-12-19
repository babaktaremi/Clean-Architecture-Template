using System.ComponentModel.DataAnnotations;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Users.Commands.Create
{
   public class UserCreateCommand : IRequest<OperationResult<UserCreateCommandResult>>
   {
       [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
   }

}
