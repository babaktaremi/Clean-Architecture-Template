using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Profiles;
using Domain.Entities.User;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers.Model
{
    public record GetUsersQueryResponseModel : ICreateMapper<User>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
    }
}
