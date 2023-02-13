using System.Text;
using CleanArc.Application.Models.Common;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Common;

public class ValidateCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : class where TRequest : IRequest<TResponse>
{
    private readonly IList<IValidator<TRequest>> _validators;

    public ValidateCommandBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToList();
    }


    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest,TResponse> next)
    {
        var errors = _validators
            .Select(v => v.Validate(message))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (!errors.Any())
            return await next(message, cancellationToken);

        throw new ValidationException(errors);
    }
}