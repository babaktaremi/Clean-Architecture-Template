using CleanArc.Application.Models.Common;
using MediatR;

namespace CleanArc.Application.Features.Users.Queries.GetUsers.Model;

public record GetUsersQueryModel : IRequest<OperationResult<List<GetUsersQueryResponseModel>>>;