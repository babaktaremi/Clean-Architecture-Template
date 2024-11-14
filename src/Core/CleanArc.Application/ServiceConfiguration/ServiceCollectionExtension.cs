using System.Reflection;
using CleanArc.Application.Common;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.Application.ServiceConfiguration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
            options.Namespace = "CleanArc.Application.Mediator";
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidateCommandBehavior<,>));

        
        services.AddAutoMapper(expression =>
        {
            expression.AddMaps(Assembly.GetExecutingAssembly());
        });


        return services;
    }

   
}