using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.Create;

public record UserCreateCommand(string UserName,string FirstName,string LastName,string PhoneNumber) : IRequest<OperationResult<UserCreateCommandResult>>;