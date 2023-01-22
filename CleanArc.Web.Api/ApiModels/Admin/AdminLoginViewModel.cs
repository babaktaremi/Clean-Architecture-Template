using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.Admin
{
    public record AdminLoginViewModel(
        [Required(ErrorMessage = "Please Enter Your Username")]
        string UserName, 
        [Required(ErrorMessage = "Please Enter Your Password")]
        string Password);
}
