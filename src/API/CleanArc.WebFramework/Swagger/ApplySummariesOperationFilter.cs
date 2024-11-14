using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using Pluralize.NET;


namespace CleanArc.WebFramework.Swagger;

public class ApplySummariesOperationFilter : IOperationProcessor
{
  

    public bool Process(OperationProcessorContext context)
    {

        if (context.ControllerType is null)
            return true;
        
        var actionName = context.MethodInfo.Name;
        var controllerName = context.ControllerType.Name.Replace("Controller", "");
        
        var pluralizer = new Pluralizer();
        
        var singularizeName =pluralizer.Singularize(controllerName);
        var pluralizeName = pluralizer.Pluralize(singularizeName);

        var parameterCount = context.OperationDescription.Operation.Parameters
                              .Count(p => p.Name != "version" && p.Name != "api-version");

        if (IsGetAllAction(actionName, pluralizeName, singularizeName, parameterCount))
        {
            if (string.IsNullOrEmpty(context.OperationDescription.Operation.Summary))
            {
                context.OperationDescription.Operation.Summary = $"Returns all {pluralizeName}";
            }
        }
        else if (IsActionName(actionName, singularizeName, "Post", "Create"))
        {
            if (string.IsNullOrEmpty(context.OperationDescription.Operation.Summary))
            {
                context.OperationDescription.Operation.Summary = $"Creates a {singularizeName}";
            }

            if (context.OperationDescription.Operation.Parameters.Count > 0 &&
                string.IsNullOrEmpty(context.OperationDescription.Operation.Parameters[0].Description))
            {
                context.OperationDescription.Operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
        }
        else if (IsActionName(actionName, singularizeName, "Read", "Get"))
        {
            if (string.IsNullOrEmpty(context.OperationDescription.Operation.Summary))
            {
                context.OperationDescription.Operation.Summary = $"Retrieves a {singularizeName} by unique id";
            }

            if (context.OperationDescription.Operation.Parameters.Count > 0 &&
                string.IsNullOrEmpty(context.OperationDescription.Operation.Parameters[0].Description))
            {
                context.OperationDescription.Operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
            }
        }
        else if (IsActionName(actionName, singularizeName, "Put", "Edit", "Update"))
        {
            if (string.IsNullOrEmpty(context.OperationDescription.Operation.Summary))
            {
                context.OperationDescription.Operation.Summary = $"Updates a {singularizeName} by unique id";
            }

            if (context.OperationDescription.Operation.Parameters.Count > 0 &&
                string.IsNullOrEmpty(context.OperationDescription.Operation.Parameters[0].Description))
            {
                context.OperationDescription.Operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
        }
        else if (IsActionName(actionName, singularizeName, "Delete", "Remove"))
        {
            if (string.IsNullOrEmpty(context.OperationDescription.Operation.Summary))
            {
                context.OperationDescription.Operation.Summary = $"Deletes a {singularizeName} by unique id";
            }

            if (context.OperationDescription.Operation.Parameters.Count > 0 &&
                string.IsNullOrEmpty(context.OperationDescription.Operation.Parameters[0].Description))
            {
                context.OperationDescription.Operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
            }
        }

        return true;
    }
    private bool IsActionName(string actionName, string singularizeName, params string[] names)
    {
        foreach (var name in names)
        {
            if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsGetAllAction(string actionName, string pluralizeName, string singularizeName, int parameterCount)
    {
        var actionNames = new[] { "Get", "Read", "Select" };
        foreach (var name in actionNames)
        {
            if ((actionName.Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0) ||
                actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

}