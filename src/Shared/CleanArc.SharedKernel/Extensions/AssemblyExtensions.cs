using System.Reflection;

namespace CleanArc.SharedKernel.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Assembly> GetDependentAssemblies(this Assembly analyzedAssembly)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => GetNamesOfAssembliesReferencedBy(a)
                .Contains(analyzedAssembly.FullName));
    }

    public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
    {
        return assembly.GetReferencedAssemblies()
            .Select(assemblyName => assemblyName.FullName);
    }
}