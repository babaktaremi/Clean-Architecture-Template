using Microsoft.Build.Framework;

namespace CleanArc.Web.Api.ApiModels.User;

public class AdminTokenRequestViewModel
{
    [Required]
    public string  UserName { get; set; }

    [Required]
    public string Password { get; set; }
}