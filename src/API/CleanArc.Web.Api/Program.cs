using System.Diagnostics;
using Asp.Versioning.Builder;
using Carter;
using CleanArc.Application.Models.Common;
using CleanArc.Application.ServiceConfiguration;
using CleanArc.Domain.Entities.User;
using CleanArc.Infrastructure.CrossCutting.Logging;
using CleanArc.Infrastructure.Identity.Identity.Dtos;
using CleanArc.Infrastructure.Identity.Identity.SeedDatabaseService;
using CleanArc.Infrastructure.Identity.Jwt;
using CleanArc.Infrastructure.Identity.ServiceConfiguration;
using CleanArc.Infrastructure.Persistence;
using CleanArc.Infrastructure.Persistence.ServiceConfiguration;
using CleanArc.SharedKernel.Extensions;
using CleanArc.SharedKernel.ValidationBase;
using CleanArc.SharedKernel.ValidationBase.Contracts;
using CleanArc.Web.Api.Controllers.V1.UserManagement;
using CleanArc.Web.Plugins.Grpc;
using CleanArc.WebFramework.EndpointFilters;
using CleanArc.WebFramework.Filters;
using CleanArc.WebFramework.Middlewares;
using CleanArc.WebFramework.ServiceConfiguration;
using CleanArc.WebFramework.Swagger;
using CleanArc.WebFramework.WebExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddCarter(configurator: configurator => { configurator.WithEmptyValidators();});

builder.Services.AddApplicationServices()
    .RegisterIdentityServices(identitySettings)
    .AddPersistenceServices(configuration)
    .AddWebFrameworkServices();

builder.Services.RegisterValidatorsAsServices();
builder.Services.AddExceptionHandler<ExceptionHandler>();


#region Plugin Services Configuration

builder.Services.ConfigureGrpcPluginServices();

#endregion

builder.Services.AddAutoMapper(typeof(User), typeof(JwtService), typeof(UserController));

var app = builder.Build();


await app.ApplyMigrationsAsync();
await app.SeedDefaultUsersAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler(_=>{});
app.UseSwaggerAndUI();

app.MapCarter();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.ConfigureGrpcPipeline();

await app.RunAsync();



