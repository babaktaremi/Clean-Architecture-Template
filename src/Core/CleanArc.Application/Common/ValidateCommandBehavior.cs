using System.Text;
using CleanArc.Application.Models.Common;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace CleanArc.Application.Common;

public class ValidateCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : class where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidateCommandBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var errors = new List<ValidationFailure>();


        foreach (var validator in _validators)
        {
            var validationResult =
                await validator.ValidateAsync(new ValidationContext<TRequest>(message), cancellationToken);

            if (!validationResult.IsValid)
                errors.AddRange(validationResult.Errors);
        }

        if (errors.Any())
            throw new ValidationException(errors);


        return await next(message, cancellationToken);
    }
}