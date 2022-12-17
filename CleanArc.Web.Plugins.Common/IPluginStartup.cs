using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.Web.Plugins.Common;

public interface IPluginStartup
{
    void ConfigureServices(IServiceCollection services);

    void ConfigurePipeline(WebApplication app);
}