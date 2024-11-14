using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace CleanArc.Infrastructure.Monitoring.Configurations;

public static class OpenTelemetryConfigurations
{
    public static WebApplicationBuilder SetupOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder.AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting"
                        , "Microsoft.AspNetCore.Server.Kestrel"
                        , "System.Net.Http"
                        , "CleanArc.Web.Api"
                        , "ControllerMetrics")
                    .AddPrometheusExporter();
            });

        builder.Services.AddMetrics();
        
        return builder;
    }
}