using System.Text.RegularExpressions;
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

        validator.RuleFor(c => c.UserName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter your username");

        validator
            .RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have last name");


        validator.RuleFor(c => c.PhoneNumber).NotEmpty()
            .NotNull().WithMessage("Phone Number is required.")
            .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
            .Matches(new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")).WithMessage("Phone number is not valid");

        return validator;
    }
}