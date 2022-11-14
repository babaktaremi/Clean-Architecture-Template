using System.Diagnostics;
using System.Reflection;
using Application.Contracts;
using Application.ServiceConfiguration;
using Domain.Entities.User;
using Identity.Identity.Dtos;
using Identity.Identity.SeedDatabaseService;
using Identity.Jwt;
using Identity.ServiceConfiguration;
using InfrastructureServices.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using Persistence.Repositories;
using Persistence.ServiceConfiguration;
using Serilog;
using Web.Api.Controllers;
using Web.Api.Controllers.V1;
using WebFramework.Filters;
using WebFramework.ServiceConfiguration;
using WebFramework.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

var configuration = builder.Configuration;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

builder.Services.Configure<IdentitySettings>(configuration.GetSection(nameof(IdentitySettings)));

var identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(OkResultAttribute));
    options.Filters.Add(typeof(NotFoundResultAttribute));
    options.Filters.Add(typeof(ContentResultFilterAttribute));
    options.Filters.Add(typeof(ModelStateValidationAttribute));
    options.Filters.Add(typeof(BadRequestResultFilterAttribute));

}).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
//.AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<UserCreateCommand>(); }); //Uncomment for FluentValidation in Application Layer

builder.Services.AddSwagger();

builder.Services.AddApplicationServices().RegisterIdentityServices(identitySettings)
    .AddPersistenceServices(configuration).AddWebFrameworkServices();

builder.Services.AddAutoMapper(typeof(User), typeof(JwtService), typeof(UserController));

var app = builder.Build();


#region Seeding and creating database

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

    if (context is null)
        throw new Exception("Database Context Not Found");

    await context.Database.MigrateAsync();


    var seedService = scope.ServiceProvider.GetRequiredService<ISeedDataBase>();
    await seedService.Seed();
}

#endregion

#region Pipleline Configuration

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerAndUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

await app.RunAsync();
#endregion


