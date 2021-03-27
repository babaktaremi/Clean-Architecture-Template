using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence;

namespace Identity.Identity.Store
{
    public class RoleStore:RoleStore<Role,ApplicationDbContext,int,UserRole,RoleClaim>
    {
        public RoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
