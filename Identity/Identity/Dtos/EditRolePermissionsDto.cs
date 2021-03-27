using System.Collections.Generic;

namespace Identity.Identity.Dtos
{
   public class EditRolePermissionsDto
    {
        public int RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
