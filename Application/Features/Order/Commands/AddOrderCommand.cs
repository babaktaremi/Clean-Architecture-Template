using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Order.Commands
{
    public record AddOrderCommand(int UserId, string OrderName) : IRequest<OperationResult<bool>>;
}
