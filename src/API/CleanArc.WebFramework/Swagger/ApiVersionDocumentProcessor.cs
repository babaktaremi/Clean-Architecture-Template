using CleanArc.SharedKernel.Extensions;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace CleanArc.WebFramework.Swagger;

public class ApiVersionDocumentProcessor: IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        // Filter out operations that do not match the current document version
        var version = context.Document.Info.Version; // e.g., "v1"

        var pathsToRemove = context.Document.Paths
            .Where(pathItem => !RegExHelpers.MatchesApiVersion( version,pathItem.Key))
            .Select(path => path.Key)
            .ToList();

        
        foreach (var path in pathsToRemove)
        {
            context.Document.Paths.Remove(path);
        }
    }
}