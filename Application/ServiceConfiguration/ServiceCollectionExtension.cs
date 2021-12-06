using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServiceConfiguration
{
   public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
