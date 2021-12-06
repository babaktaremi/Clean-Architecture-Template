using Application.Models.ApiResult;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebFramework.Middlewares
{
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
            catch (Exception exception)
            {
                _logger.LogError(exception,exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (!_env.IsDevelopment())
                {
                    context.Response.ContentType = "application/json";
                    var response = new ApiResult(false,
                        ApiResultStatusCode.ServerError, "Server Error");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(response);
                }

                //await _next(context);
            }
        }
    }
}
