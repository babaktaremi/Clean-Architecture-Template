using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace CleanArc.Infrastructure.Monitoring.Configurations;

public static class PrometheusMetricsConfigurations
{
    public static WebApplication UseMetrics(this WebApplication app)
    {

        app.UseMetricServer();
        app.UseHttpMetrics();
        return app;
    }
}