using System.Reflection;
using CleanArc.Utils;
using CleanArc.Web.Plugins.Common;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.WebFramework.ServiceConfiguration;

public static class PluginServiceCollectionExtensions
{
    public static ApplicationPartManager  AddPluginParts(this ApplicationPartManager applicationPartManager)
    {
        var commonPluginAssembly = typeof(IPluginStartup).Assembly;

        var dependentPluginAssemblies = commonPluginAssembly.GetDependentAssemblies().Where(c=>
        {
            var assemblyFullName = typeof(PluginServiceCollectionExtensions).Assembly.FullName;
            return assemblyFullName != null && c.FullName != null &&
                   !c.FullName.Contains(assemblyFullName);
        }).ToList();


        foreach (var dependentPluginAssembly in dependentPluginAssemblies)
        {
            var targetAssembly =
                Assembly.LoadFrom(dependentPluginAssembly.Location);

            var part = new AssemblyPart(targetAssembly);

            applicationPartManager.ApplicationParts.Add(part);

        }

        return applicationPartManager;
    }

    public static IServiceCollection AddPluginServices(this IServiceCollection services)
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

            var pluginStartup= targetAssembly.DefinedTypes.FirstOrDefault(c => c.GetInterfaces().Any(x =>
                x == typeof(IPluginStartup)));


            var method = pluginStartup.GetMethods().Single(c => c.Name == "ConfigureServices");

            var cacheClass = Activator.CreateInstance(pluginStartup);

            method.Invoke(cacheClass, new object[] { services }) ;
        }

        return services;
    }
}