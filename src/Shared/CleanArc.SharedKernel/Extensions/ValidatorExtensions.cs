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

            foreach (var type in types)
            {
                var typeConstructorArgumentLength = type.GetConstructors().First().GetParameters().Length;
                var model = Activator.CreateInstance(type, new object[typeConstructorArgumentLength]);

                var methodInfo = type.GetMethod(nameof(IValidatableModel<object>.ValidateApplicationModel));

                if (model != null)
                {
                    var methodArgument = Activator.CreateInstance(typeof(ApplicationBaseValidationModel<>).MakeGenericType(type));
                    var validator = methodInfo?.Invoke(model, new[] { methodArgument });

                    if (validator != null)
                    {
                        var interfaces = validator.GetType().GetInterfaces();

                        var validatorInterface = interfaces
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                        if (validatorInterface != null)
                            services.Add(new ServiceDescriptor(validatorInterface, validator));
                    }
                }
            }
            // Needed Configuration for MicroElements.Swashbuckle.FluentValidation . But it will be obsolete in near future ...
#pragma warning disable CS0618 // Type or member is obsolete
            services.TryAddSingleton<IValidatorFactory, ServiceProviderValidatorFactory>();
#pragma warning restore CS0618 // Type or member is obsolete
            return services;
        }
    }

}
