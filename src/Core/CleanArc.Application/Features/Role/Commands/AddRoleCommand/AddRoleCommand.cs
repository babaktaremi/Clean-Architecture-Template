using CleanArc.Application.Models.Common;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Role.Commands.AddRoleCommand;

public record AddRoleCommand(string RoleName) : IRequest<OperationResult<bool>>,
    IValidatableModel<AddRoleCommand>
{
    public IValidator<AddRoleCommand> ValidateApplicationModel(ApplicationBaseValidationModelProvider<AddRoleCommand> validator)
    {
        validator
            .RuleFor(c => c.RoleName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter role name");

        return validator;
    }
};