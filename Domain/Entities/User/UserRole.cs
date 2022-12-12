using CleanArc.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArc.Domain.Entities.User;

public class UserRole : IdentityUserRole<int>,IEntity
{
    public User User { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedUserRoleDate { get; set; }

}