using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArc.WebFramework.Swagger;

public class CustomTokenRequiredOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<RequireTokenWithoutAuthorizationAttribute> filter;

    public CustomTokenRequiredOperationFilter()
    {
        this.filter =
            new SecurityRequirementsOperationFilter<RequireTokenWithoutAuthorizationAttribute>(
                _ => Array.Empty<string>(), false);
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context) => this.filter.Apply(operation, context);


}
