using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CleanArc.WebFramework.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        //More info : https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters


        //Add services and configuration to use swagger
        services.AddSwaggerGen(options =>
        {
            var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "Project.xml");
            //show controller XML comments like summary
            options.IncludeXmlComments(xmlDocPath, true);

            options.EnableAnnotations();

            #region DescribeAllEnumsAsStrings
            //This method was Deprecated. 

            //You can specify an enum to convert to/from string, uisng :
            //[JsonConverter(typeof(StringEnumConverter))]
            //public virtual MyEnums MyEnum { get; set; }

            //Or can apply the StringEnumConverter to all enums globaly, using :
            //SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            //OR
            //JsonConvert.DefaultSettings = () =>
            //{
            //    var settings = new JsonSerializerSettings();
            //    settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            //    return settings;
            //};
            #endregion

            //options.DescribeAllParametersInCamelCase();
            //options.DescribeStringEnumsInCamelCase()
            //options.UseReferencedDefinitionsForEnums()
            //options.IgnoreObsoleteActions();
            //options.IgnoreObsoleteProperties();

            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });
            options.SwaggerDoc("v1.1", new OpenApiInfo { Version = "v1.1", Title = "API V1.1 using minimal API endpoints" });

            #region Filters
            //Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
            //options.ExampleFilters();

            //It doesn't work anymore in recent versions because of replacing Swashbuckle.AspNetCore.Examples with Swashbuckle.AspNetCore.Filters
            //Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
            //options.OperationFilter<AddFileParamTypesOperationFilter>();

            //Set summary of action if not already set
            options.OperationFilter<ApplySummariesOperationFilter>();

            #region Add UnAuthorized to Response
            //Add 401 response and security requirements (Lock icon) to actions that need authorization
            #endregion

            #region Add Jwt Authentication
            //Add Lockout icon on top of swagger ui page to authenticate
            #region Old way
            //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //{
            //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //    Name = "Authorization",
            //    In = "header"
            //});
            //options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            //{
            //    {"Bearer", new string[] { }}
            //});
            #endregion

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \" {token}\"",
                In =ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.OperationFilter<CustomTokenRequiredOperationFilter>();


            //OAuth2Scheme
            //options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme());
            #endregion

            #region Versioning
            // Remove version parameter from all Operations
            options.OperationFilter<RemoveVersionParameters>();

            //set version "api/v{version}/[controller]" from current swagger doc verion
            options.DocumentFilter<SetVersionInPaths>();

            //Seperate and categorize end-points by doc version
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
               
                var endpointApiVersions = apiDesc.ActionDescriptor.EndpointMetadata.OfType<ApiVersionMetadata>();

                foreach (var endpointApiVersion in endpointApiVersions)
                {
                    endpointApiVersion.Deconstruct(out _, out var endpointModel);

                   return endpointModel.DeclaredApiVersions.Any(version => $"v{version.ToString()}" == docName);

                }

                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                var versions = methodInfo.DeclaringType
                    .GetCustomAttributes<ApiVersionAttribute>(true)
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v.ToString()}" == docName);
            });
            #endregion

            #endregion
        });
    }

    public static void UseSwaggerAndUI(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        //More info : https://github.com/domaindrivendev/Swashbuckle.AspNetCore

        //Swagger middleware for generate "Open API Documentation" in swagger.json
        app.UseSwagger(options =>
        {
         //   options.RouteTemplate = "api-docs/{documentName}/swagger.json";
        });

        //Swagger middleware for generate UI from swagger.json
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            options.SwaggerEndpoint("/swagger/v1.1/swagger.json", "V1.1 Docs Using Minimal Api Endpoints");

            #region Customizing
            //// Display
            //options.DefaultModelExpandDepth(2);
            //options.DefaultModelRendering(ModelRendering.Model);
            //options.DefaultModelsExpandDepth(-1);
            //options.DisplayOperationId();
            //options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.None);
            //options.EnableDeepLinking();
            //options.EnableFilter();
            //options.MaxDisplayedTags(5);
            //options.ShowExtensions();

            //// Network
            //options.EnableValidator();
            //options.SupportedSubmitMethods(SubmitMethod.Get);

            //// Other
            //options.DocumentTitle = "CustomUIConfig";
            //options.InjectStylesheet("/ext/custom-stylesheet.css");
            //options.InjectJavascript("/ext/custom-javascript.js");
            //options.RoutePrefix = "api-docs";
            #endregion
        });

        //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
        app.UseReDoc(options =>
        {
            options.SpecUrl("/swagger/v1/swagger.json");
            //options.SpecUrl("/swagger/v2/swagger.json");

            #region Customizing
            //By default, the ReDoc UI will be exposed at "/api-docs"
            //options.RoutePrefix = "docs";
            //options.DocumentTitle = "My API Docs";

            options.EnableUntrustedSpec();
            options.ScrollYOffset(10);
            options.HideHostname();
            options.HideDownloadButton();
            options.ExpandResponses("200,201");
            options.RequiredPropsFirst();
            options.NoAutoAuth();
            options.PathInMiddlePanel();
            options.HideLoading();
            options.NativeScrollbars();
            options.DisableSearch();
            options.OnlyRequiredInSamples();
            options.SortPropsAlphabetically();
            #endregion
        });
    }
}