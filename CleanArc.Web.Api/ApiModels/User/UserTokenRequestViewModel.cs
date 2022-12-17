using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.User;

public class UserTokenRequestViewModel
{
    [Phone]
    [Required]
    public string UserPhoneNumber { get; set; }
}