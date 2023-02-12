using System.Text;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Common;

public class ValidateCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : class where TRequest : IRequest<TResponse>
{
    private readonly IList<IValidator<TRequest>> _validators;

    public ValidateCommandBehavior(IList<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var errors = _validators
            .Select(v => v.Validate(message))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
        {
            var errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("Invalid command, reason: ");

            foreach (var error in errors)
            {
                errorBuilder.AppendLine(error.ErrorMessage);
            }

            throw new Exception(errorBuilder.ToString());

        }

        return await next(message,cancellationToken);
    }
}