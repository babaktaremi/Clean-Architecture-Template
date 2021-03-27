using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User
{
    public class UserClaim:IdentityUserClaim<int>,IEntity
    {
        public User User { get; set; }
    }
  
}
