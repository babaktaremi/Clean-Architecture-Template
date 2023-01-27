using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.User;

public class CreateUserViewModel
{
    [Required]
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
}