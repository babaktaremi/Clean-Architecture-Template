using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Admin.Queries.GetToken;

public record AdminGetTokenQuery(string UserName, string Password) : IRequest<OperationResult<AccessToken>>,
    IValidatableModel<AdminGetTokenQuery>
{
    public IValidator<AdminGetTokenQuery> ValidateApplicationModel(ApplicationBaseValidationModelProvider<AdminGetTokenQuery> validator)
    {
        validator.RuleFor(c => c.UserName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter admin username");

        validator.RuleFor(c => c.Password)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter admin password");

        return validator;
    }
};