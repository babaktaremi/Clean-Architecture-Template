using System.Net.Security;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace CleanArc.Infrastructure.Monitoring.Configurations;

public static class HealthCheckConfigurations
{
    public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!, name: "SQL Server")
            .ForwardToPrometheus();

        var currentUrl = builder.Configuration["ASPNETCORE_URLS"]?.Split(';')[0].Replace("+", "localhost");
        

        return builder;
    }

    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/HealthCheck", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).ShortCircuit().DisableHttpMetrics();

        

        return app;
    }
}