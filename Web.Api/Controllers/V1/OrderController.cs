using Application.Features.Order.Commands;
using Application.Features.Order.Queries.GetUserOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.ApiModels.Order;
using WebFramework.BaseController;

namespace Web.Api.Controllers.V1
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/User")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("CreateNewOrder")]
        public async Task<IActionResult> CreateNewOrder(CreateOrderModel model)
        {
            var command = await _sender.Send(new AddOrderCommand(base.UserId, model.OrderName));

            return base.OperationResult(command);
        }

        [HttpGet("GetUserOrders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var query = await _sender.Send(new GetUserOrdersQueryModel(base.UserId));

            return base.OperationResult(query);
        }
    }
}
