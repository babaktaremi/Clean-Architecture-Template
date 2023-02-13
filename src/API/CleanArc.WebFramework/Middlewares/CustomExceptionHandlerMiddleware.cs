using Azure;
using CleanArc.Application.Models.ApiResult;
using CleanArc.SharedKernel.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArc.WebFramework.Middlewares;

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {


        try
        {
            await _next(context);
        }

        catch (ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            var errors = new Dictionary<string, List<string>>();

            foreach (var validationExceptionError in validationException.Errors)
            {
                if(!errors.ContainsKey(validationExceptionError.PropertyName))
                    errors.Add(validationExceptionError.PropertyName,new List<string>(){validationExceptionError.ErrorMessage});
                else
                    errors[validationExceptionError.PropertyName].Add(validationExceptionError.ErrorMessage);

            }

            var apiResult = new ApiResult<IDictionary<string, List<string>>>(false, ApiResultStatusCode.EntityProcessError, errors, ApiResultStatusCode.EntityProcessError.ToDisplay());

            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(apiResult);
        }

        catch (Exception exception)
        {
            _logger.LogError(exception,exception.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (!_env.IsDevelopment())
            {
                context.Response.ContentType = "application/problem+json";
                var response = new ApiResult(false,
                    ApiResultStatusCode.ServerError, "Server Error");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }

            //await _next(context);
        }
    }
}