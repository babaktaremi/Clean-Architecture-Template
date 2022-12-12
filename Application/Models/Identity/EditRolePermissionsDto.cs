namespace CleanArc.Application.Models.Identity;

public class EditRolePermissionsDto
{
    public int RoleId { get; set; }
    public List<string> Permissions { get; set; }
}