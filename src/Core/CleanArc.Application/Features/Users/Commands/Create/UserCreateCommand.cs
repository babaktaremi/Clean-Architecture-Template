using CleanArc.Application.Common.ValidationBase;
using CleanArc.Application.Common.ValidationBase.Contracts;
using CleanArc.Application.Models.Common;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.Create;

public record UserCreateCommand
    (string UserName, string FirstName, string LastName, string PhoneNumber) : IRequest<OperationResult<UserCreateCommandResult>>,IValidatableApplicationModel<UserCreateCommand>
{
    public UserCreateCommand() : this(string.Empty,string.Empty,string.Empty,string.Empty)
    {
        
    }

    public IValidator<UserCreateCommand> ValidateApplicationModel()
    {
        var userCreationValidation = new ApplicationBaseValidationModel<UserCreateCommand>();

        userCreationValidation
            .RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have first name");

        userCreationValidation
            .RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have first name");

        return userCreationValidation;
    }
}