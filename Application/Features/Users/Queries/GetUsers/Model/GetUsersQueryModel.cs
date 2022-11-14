using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers.Model
{
    public record GetUsersQueryModel : IRequest<OperationResult<List<GetUsersQueryResponseModel>>>;
}
