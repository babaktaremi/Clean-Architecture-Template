using Microsoft.Build.Framework;

namespace CleanArc.Web.Api.ApiModels.User;

public class ConfirmUserPhoneNumberViewModel
{
    [Required]
    public string UserKey { get; set; }

    [Required]
    public string Code { get; set; }
}