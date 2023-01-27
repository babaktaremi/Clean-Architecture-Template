using CleanArc.Web.Plugins.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CleanArc.Web.Plugins.Grpc.Services;

namespace CleanArc.Web.Plugins.Grpc;

public class PluginStartup:IPluginStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //services.AddMediator(options =>
        //{
        //    options.ServiceLifetime = ServiceLifetime.Transient;
        //    options.Namespace = "CleanArc.Plugins.GRPC.Mediator";
        //});

        //services.AddMediator(Assembly.GetExecutingAssembly());
        
        services.AddGrpc();
        services.AddGrpcReflection();
    }

    public void ConfigurePipeline(WebApplication app)
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