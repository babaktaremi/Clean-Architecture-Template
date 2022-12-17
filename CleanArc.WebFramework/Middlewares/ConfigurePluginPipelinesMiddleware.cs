using CleanArc.Application.Models.ApiResult;
using CleanArc.Utils;
using CleanArc.Web.Plugins.Common;
using CleanArc.WebFramework.ServiceConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CleanArc.WebFramework.Middlewares;

public static class ConfigurePluginPipelinesMiddleware
{
    public static WebApplication ConfigurePluginsPipelines(this WebApplication app)
    {
        var commonPluginAssembly = typeof(IPluginStartup).Assembly;

        var dependentPluginAssemblies = commonPluginAssembly.GetDependentAssemblies().Where(c =>
        {
            var assemblyFullName = typeof(PluginServiceCollectionExtensions).Assembly.FullName;
            return assemblyFullName != null && c.FullName != null &&
                   !c.FullName.Contains(assemblyFullName);
        }).ToList();


        foreach (var dependentPluginAssembly in dependentPluginAssemblies)
        {
            var targetAssembly =
                Assembly.LoadFrom(dependentPluginAssembly.Location);

            var pluginStartup = targetAssembly.DefinedTypes.FirstOrDefault(c => c.GetInterfaces().Any(x =>
                x == typeof(IPluginStartup)));


            var method = pluginStartup.GetMethods().Single(c => c.Name == "ConfigurePipeline");

            var cacheClass = Activator.CreateInstance(pluginStartup);

            method.Invoke(cacheClass, new object[] { app });
        }

        return app;
    }
}

