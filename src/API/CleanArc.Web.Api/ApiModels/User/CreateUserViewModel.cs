using System.Text.RegularExpressions;
using CleanArc.Application.Features.Users.Commands.Create;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArc.Web.Api.ApiModels.User;

public class CreateUserViewModel: IValidatableModel<CreateUserViewModel>
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
   


    public IValidator<CreateUserViewModel> ValidateApplicationModel(ApplicationBaseValidationModelProvider<CreateUserViewModel> validator)
    {
        validator.RuleFor(c => c.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(10)
            .WithMessage("Minimum Length of user name must be 10 characters");

        validator
            .RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("User must have first name");

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