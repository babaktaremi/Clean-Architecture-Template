using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Users.Queries.TokenRequest;

public record PasswordUserTokenRequestQuery
(string UserName,string Password)
:IValidatableModel<PasswordUserTokenRequestQuery>,IRequest<OperationResult<AccessToken>>
{
    public IValidator<PasswordUserTokenRequestQuery> ValidateApplicationModel(ApplicationBaseValidationModelProvider<PasswordUserTokenRequestQuery> validator)
    {
        validator.RuleFor(c => c.UserName)
            .NotEmpty();

        validator.RuleFor(c => c.Password)
            .NotEmpty();

        return validator;
    }
}