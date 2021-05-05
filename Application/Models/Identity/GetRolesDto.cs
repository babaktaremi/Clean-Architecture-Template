using Application.Profiles;
using Domain.Entities.User;

namespace Application.Models.Identity
{
   public class GetRolesDto:ICreateMapper<Role>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
