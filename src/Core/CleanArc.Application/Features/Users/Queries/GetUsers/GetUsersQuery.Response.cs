
using CleanArc.Domain.Entities.User;

namespace CleanArc.Application.Features.Users.Queries.GetUsers;

public record GetUsersQueryResponse 
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
}