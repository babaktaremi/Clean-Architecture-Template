using System.Text.Json.Serialization;
using CleanArc.Application.Models.Common;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Features.Order.Commands;

public record AddOrderCommand( string OrderName) : IRequest<OperationResult<bool>>,
    IValidatableModel<AddOrderCommand>
{
    [JsonIgnore]
    public int UserId { get; set; }

    public IValidator<AddOrderCommand> ValidateApplicationModel(ApplicationBaseValidationModelProvider<AddOrderCommand> validator)
    {
        validator.RuleFor(c => c.OrderName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please enter your role name");

        return validator;
    }
}