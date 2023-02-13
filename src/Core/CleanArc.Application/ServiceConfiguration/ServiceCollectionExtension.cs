using System.Reflection;
using CleanArc.Application.Common;
using CleanArc.Application.Common.ValidationBase.Contracts;
using CleanArc.SharedKernel.Extensions;
using FluentValidation;
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
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidateCommandBehavior<,>));
        //services.AddMediator(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());


        SetupApplicationModelValidators(services);

        return services;
    }

    private static void SetupApplicationModelValidators(IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatableApplicationModel<>)))
            .ToList();

        foreach (var type in types)
        {
            var model = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("ValidateApplicationModel");

            if (model != null){
               var validator= methodInfo?.Invoke(model, null);

               if (validator != null)
               {
                   var interfaces = validator.GetType().GetInterfaces();

                   var validatorInterface = interfaces
                       .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));


                   services.Add(new ServiceDescriptor(validatorInterface, validator));
               }
            }
        }
    }
}