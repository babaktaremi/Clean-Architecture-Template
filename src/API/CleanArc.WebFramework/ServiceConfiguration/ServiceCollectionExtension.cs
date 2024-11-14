using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.WebFramework.ServiceConfiguration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddWebFrameworkServices(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            //url segment => {version}
            options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
            options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
            options.ReportApiVersions = true;

            //ApiVersion.TryParse("1.0", out var version10);
            //ApiVersion.TryParse("1", out var version1);
            //var a = version10 == version1;

            //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            // api/posts?api-version=1

            //options.ApiVersionReader = new UrlSegmentApiVersionReader();
            // api/v1/posts

            //options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
            // header => Api-Version : 1

            //options.ApiVersionReader = new MediaTypeApiVersionReader()

            //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
            // combine of [querystring] & [urlsegment]
        }).AddMvc() 
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });;

        return services;


    }
}