using Domain.Entities.User;
using Identity.Profiles;

namespace Identity.Identity.Dtos
{
   public class GetRolesDto:ICreateMapper<Role>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
