using CleanArc.Application.Profiles;
using CleanArc.Domain.Entities.User;

namespace CleanArc.Application.Features.Users.Queries.GetUsers;

public record GetUsersQueryResponse : ICreateMapper<User>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
}