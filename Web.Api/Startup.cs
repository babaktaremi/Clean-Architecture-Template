using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Users.Commands.Create;
using Application.ServiceConfiguration;
using Identity.Identity.Dtos;
using Identity.ServiceConfiguration;
using Persistence.ServiceConfiguration;
using WebFramework.Filters;
using WebFramework.ServiceConfiguration;
using WebFramework.Swagger;

namespace Web.Api
{
    public class Startup
    {
        private readonly IdentitySettings _identitySettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentitySettings>(Configuration.GetSection(nameof(IdentitySettings)));

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(OkResultAttribute));
                options.Filters.Add(typeof(NotFoundResultAttribute));
                options.Filters.Add(typeof(ContentResultFilterAttribute));
                options.Filters.Add(typeof(ModelStateValidationAttribute));
                options.Filters.Add(typeof(BadRequestResultFilterAttribute));

            }).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
                //.AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<UserCreateCommand>(); });

            services.AddSwagger();
            
            services.AddApplicationServices().RegisterIdentityServices(_identitySettings)
                .AddPersistenceServices(Configuration).AddWebFrameworkServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerAndUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
