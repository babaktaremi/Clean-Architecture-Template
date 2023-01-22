namespace CleanArc.WebFramework.Swagger;


/// <summary>
/// Marker Attribute for Custom Actions or controllers that need token but without authorization check
/// </summary>
[AttributeUsage(AttributeTargets.Class| AttributeTargets.Method)]
public class RequireTokenWithoutAuthorizationAttribute : Attribute
{

};