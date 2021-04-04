using System;
using System.Collections.Generic;
using System.Text;
using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence.ServiceConfiguration
{
   public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            return services;
        }
    }
}
