using CleanArc.Application.Models.Common;
using CleanArc.Application.Models.Jwt;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Users.Commands.RefreshUserTokenCommand;

public record RefreshUserTokenCommand(Guid RefreshToken) : IRequest<OperationResult<AccessToken>>,
    IValidatableModel<RefreshUserTokenCommand>
{
    public IValidator<RefreshUserTokenCommand> ValidateApplicationModel(ApplicationBaseValidationModelProvider<RefreshUserTokenCommand> validator)
    {
        validator.RuleFor(c => c.RefreshToken)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter valid user refresh token");

        return validator;
    }
};