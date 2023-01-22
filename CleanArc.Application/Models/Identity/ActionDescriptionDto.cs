namespace CleanArc.Application.Models.Identity;

public class ActionDescriptionDto
{
    public string Key => $"{AreaName}:{ControllerName}:{ActionName}";

    public string AreaName { get; set; }

    public string ControllerName { get; set; }
    public string ControllerDisplayName { get; set; }

    public string ActionName { get; set; }

    public string ActionDisplayName { get; set; }
    public string ControllerDescription { get; set; }
}