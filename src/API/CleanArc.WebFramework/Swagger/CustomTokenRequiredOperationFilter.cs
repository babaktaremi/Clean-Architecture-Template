using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;


namespace CleanArc.WebFramework.Swagger;

public class CustomTokenRequiredOperationFilter : IOperationProcessor
{
    
    public bool Process(OperationProcessorContext context)
    {
        var hasAttribute = context.MethodInfo
            .GetCustomAttributes(typeof(RequireTokenWithoutAuthorizationAttribute), false).Any();

        if (hasAttribute)
        {
            // Add security requirements to the operation
            var securityScheme = new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization"
            };

            var securityRequirement = new OpenApiSecurityRequirement { { securityScheme.Scheme, new List<string>() } };

            
            context.OperationDescription.Operation.Security=[securityRequirement];
        }

        return true;
    }
}
