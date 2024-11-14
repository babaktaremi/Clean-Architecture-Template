using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CleanArc.WebFramework.EndpointFilters;

public class ModelStateValidationEndpointFilter:IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var validationSummery = new Dictionary<string, List<string>>();

        foreach (var contextArgument in context.Arguments)
        {
            if (contextArgument is null)
                continue;

            var validator =
                context.HttpContext.RequestServices.GetService(
                    typeof(IValidator<>).MakeGenericType(contextArgument.GetType())) as IValidator;

            if (validator is null)
                continue;

            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(contextArgument));

            if (validationResult.IsValid) continue;

            foreach (var validationResultError in validationResult.Errors)
            {
                if (validationSummery.TryGetValue(validationResultError.PropertyName, out var value))
                {
                    value.Add(validationResultError.ErrorMessage);
                    continue;
                }

                validationSummery.Add(validationResultError.PropertyName, new (){validationResultError.ErrorMessage});
            }
        }

        if (validationSummery.Count == 0)
            return await next(context);

        return Results.BadRequest(validationSummery);
    }
}