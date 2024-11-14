using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;

namespace CleanArc.WebFramework.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static void AddSwagger(this IServiceCollection services,
        params string[] versions)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));


        foreach (var version in versions)
        {
            services.AddOpenApiDocument(options =>
            {
                options.Title = "Clean Architecture OpenAPI docs";
                options.Version = version;
                options.DocumentName = version;
               
                
                options.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme()
                {
                    Description = "Enter JWT Token ONLY",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });

                options.DocumentProcessors.Add(new ApiVersionDocumentProcessor());
                options.OperationProcessors.Add(new ApplySummariesOperationFilter());
                options.OperationProcessors.Add(new CustomTokenRequiredOperationFilter());

                options.OperationProcessors.Add(
                    new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                
                
            });
        }
    }

    public static void UseSwaggerAndUi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        app.UseOpenApi();

        app.UseSwaggerUi(options =>
        {
            options.PersistAuthorization = true;

            options.EnableTryItOut = true;

            options.Path = "/swagger";
            
        });

        app.UseReDoc(settings =>
        {
            settings.Path = "/api-docs/{documentName}";
            settings.DocumentTitle = "Clean Architecture API doc sample";
        });
    }
}