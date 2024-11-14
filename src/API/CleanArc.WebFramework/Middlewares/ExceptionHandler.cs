using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using CleanArc.SharedKernel.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArc.WebFramework.Middlewares;

public class ExceptionHandler(ILogger<ExceptionHandler> logger,IWebHostEnvironment environment) : IExceptionHandler
{
    
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (environment.IsDevelopment())
            return false;

        if (exception is FluentValidation.ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            var errors = new Dictionary<string, List<string>>();

            foreach (var validationExceptionError in validationException.Errors)
            {
                if (!errors.ContainsKey(validationExceptionError.PropertyName))
                    errors.Add(validationExceptionError.PropertyName, new List<string>() { validationExceptionError.ErrorMessage });
                else
                    errors[validationExceptionError.PropertyName].Add(validationExceptionError.ErrorMessage);

            }

            var apiResult = new ApiResult<IDictionary<string, List<string>>>(false, ApiResultStatusCode.EntityProcessError, errors, ApiResultStatusCode.EntityProcessError.ToDisplay());

            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(apiResult, cancellationToken: cancellationToken);

            return true;
        }

        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature is not null)
            logger.LogError(exceptionFeature.Error,
                "Unhandled exception occured. Path: {exceptionUrlPath} ."
                , exceptionFeature.Path
            );

        else
            logger.LogError(exception, "Error captured in global exception handler");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/problem+json";
        var response = new ApiResult(false,
            ApiResultStatusCode.ServerError, "Internal Server Error");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);

        return true;
    }
}