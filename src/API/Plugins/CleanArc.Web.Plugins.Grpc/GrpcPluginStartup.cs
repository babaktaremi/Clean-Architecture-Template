using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CleanArc.Web.Plugins.Grpc.Services;

namespace CleanArc.Web.Plugins.Grpc;

public static class GrpcPluginStartup
{
    public static IServiceCollection ConfigureGrpcPluginServices(this IServiceCollection services)
    {
     
        
        services.AddGrpc();
        services.AddGrpcReflection();

        return services;
    }

    public static void ConfigureGrpcPipeline(this WebApplication app)
    {

        app.MapGrpcService<UserGrpcServices>();
        app.MapGrpcService<OrderGrpcServices>();
        app.MapGrpcReflectionService();

        app.MapGet("/GrpcUser", async context =>
        {
            await context.Response.WriteAsync(
                "Communication with this gRPC endpoint must be made through a gRPC client.");
        });

        app.MapGet("/GrpcUserOrder", async context =>
        {
            await context.Response.WriteAsync(
                "Communication with this gRPC endpoint must be made through a gRPC client.");
        });
    }
}