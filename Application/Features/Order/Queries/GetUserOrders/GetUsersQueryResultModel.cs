using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Order.Queries.GetUserOrders
{
    public record GetUsersQueryResultModel(int OrderId, string OrderName);
}
