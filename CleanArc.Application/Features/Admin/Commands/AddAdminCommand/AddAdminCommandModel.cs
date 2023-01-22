using CleanArc.Application.Models.Common;
using Mediator;

namespace CleanArc.Application.Features.Admin.Commands.AddAdminCommand;

public record AddAdminCommandModel(string UserName,string Email,string Password,int RoleId):IRequest<OperationResult<bool>>;