using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArc.SharedKernel.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection RegisterValidatorsAsServices(this IServiceCollection services)
        {

            var types = AppDomain.CurrentDomain.GetAssemblies().Where(c => c != typeof(ValidatorExtensions).Assembly).SelectMany(a => a.GetExportedTypes()).Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatableModel<>)))
                .ToList();

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions(){ValidateScopes = true}).CreateScope();


            foreach (var type in types)
            {
                var typeConstructorArgumentLength = type.GetConstructors().First().GetParameters().Length;
                var model = Activator.CreateInstance(type, new object[typeConstructorArgumentLength]);

                var methodInfo = type.GetMethod(nameof(IValidatableModel<object>.ValidateApplicationModel));


                if (model != null)
                {
                    var methodArgument = Activator.CreateInstance(typeof(ApplicationBaseValidationModelProvider<>).MakeGenericType(type), serviceProvider);
                    var validator = methodInfo?.Invoke(model, new[] { methodArgument });

                    if (validator != null)
                    {
                        var interfaces = validator.GetType().GetInterfaces();

                        //services.AddSingleton(validator.GetType(),validator);

                        var validatorInterface = interfaces
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                        if (validatorInterface != null)
                            services.AddScoped(validatorInterface, _ => validator);

                    }
                }
            }
            return services;
        }
    }

}
