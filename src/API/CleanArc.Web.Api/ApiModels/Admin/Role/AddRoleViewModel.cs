using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.Admin.Role
{
    public record AddRoleViewModel([Required(ErrorMessage = "Please enter role name")]
        string RoleName);
}
