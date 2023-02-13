using CleanArc.WebFramework.Validations.Contracts;
using System.Text.RegularExpressions;
using CleanArc.WebFramework.Validations;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArc.Web.Api.ApiModels.User;

public class CreateUserViewModel: IValidatableViewModel
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
    public ValidationResult ValidateRules()
    {
        var validationModel = new ValidationModelBase<CreateUserViewModel>();

        validationModel.RuleFor(c => c.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(5)
            .WithMessage("Minimum Length of user name must be 5 characters");
        
        validationModel.RuleFor(c=>c.PhoneNumber).NotEmpty()
            .NotNull().WithMessage("Phone Number is required.")
            .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
            .Matches(new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")).WithMessage("PhoneNumber not valid");

        return validationModel.Validate(new ValidationContext<CreateUserViewModel>(this));
    }

}