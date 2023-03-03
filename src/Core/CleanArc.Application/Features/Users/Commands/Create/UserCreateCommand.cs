using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.Application.Features.Users.Commands.Create;

public record UserCreateCommand
    (string UserName, string FirstName, string LastName, string PhoneNumber) : IRequest<OperationResult<UserCreateCommandResult>>,IValidatableModel<UserCreateCommand>
{

    public IValidator<UserCreateCommand> ValidateApplicationModel(ApplicationBaseValidationModelProvider<UserCreateCommand> validator)
    {

        validator
            .RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have first name");


        var userManager = validator.ServiceProvider.ServiceProvider.GetRequiredService<IAppUserManager>();

        validator.RuleFor(c => c.UserName)
            .MustAsync(async (userName, _) => !await userManager.IsExistUserName(userName))
            .WithMessage("Username is already taken. try another username");


        validator
            .RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have last name");

        return validator;
    }
}