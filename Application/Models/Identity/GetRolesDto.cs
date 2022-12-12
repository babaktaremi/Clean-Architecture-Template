using CleanArc.Application.Profiles;
using CleanArc.Domain.Entities.User;

namespace CleanArc.Application.Models.Identity;

public class GetRolesDto:ICreateMapper<Role>
{
    public string Id { get; set; }
    public string Name { get; set; }
}