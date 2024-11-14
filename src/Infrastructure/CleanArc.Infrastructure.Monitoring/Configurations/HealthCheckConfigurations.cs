using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace CleanArc.Infrastructure.Monitoring.Configurations;

public static class HealthCheckConfigurations
{
    public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!, name: "SQL Server")
            .ForwardToPrometheus();
        
        builder.Services.AddHealthChecksUI(settings =>
        {
            settings.MaximumHistoryEntriesPerEndpoint(1000);
            settings.SetMinimumSecondsBetweenFailureNotifications(5);
            settings.SetEvaluationTimeInSeconds(5);
            settings.AddHealthCheckEndpoint("Clean Architecture Health Checks", "/Healthcheck");
            settings.ConfigureApiEndpointHttpclient((_, client) =>
            {
                client.DefaultRequestVersion = new Version(2, 0);
            });
        }).AddInMemoryStorage();

        return builder;
    }

    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        
        app.MapHealthChecks("/HealthCheck", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).ShortCircuit().DisableHttpMetrics();
        
        app.MapHealthChecksUI(options =>
        {
            options.ApiPath = "/HealthcheckApi";
            options.UseRelativeApiPath = true;
            options.UIPath = "/HealthCheckUi";
        }).ShortCircuit()
            .DisableHttpMetrics();

        return app;
    }
}