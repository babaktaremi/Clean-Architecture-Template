using CleanArc.Application.Features.Order.Queries.GetUserOrders;
using CleanArc.SharedKernel.Extensions;
using CleanArc.Web.Plugins.Grpc.ProtoModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mediator;
using Microsoft.AspNetCore.Authorization;

namespace CleanArc.Web.Plugins.Grpc.Services
{
    [Authorize]
    public class OrderGrpcServices:OrderServices.OrderServicesBase
    {
       

        private readonly ISender _sender;

        public OrderGrpcServices(ISender sender)
        {
            _sender = sender;
        }

        public override async Task GetUserOrders(Empty request, IServerStreamWriter<GetUserOrdersModel> responseStream, ServerCallContext context)
        {
            var userId = int.Parse(context.GetHttpContext().User.Identity.GetUserId());

            var query = await _sender.Send(new GetUserOrdersQueryModel(userId));

            if (!query.IsSuccess)
            {
                context.Status = new Status(StatusCode.InvalidArgument, query.GetErrorMessage());
                return;
            }

            foreach (var getUsersQueryResultModel in query.Result)
            {
                    await responseStream.WriteAsync(new GetUserOrdersModel()
                        { OrderId = getUsersQueryResultModel.OrderId, OrderName = getUsersQueryResultModel.OrderName });

                    await Task.Delay(400);
                
            }

        }
    }
}
