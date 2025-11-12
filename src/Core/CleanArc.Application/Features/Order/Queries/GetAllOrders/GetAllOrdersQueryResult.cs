

using Mapster;

namespace CleanArc.Application.Features.Order.Queries.GetAllOrders;

public record GetAllOrdersQueryResult(int OrderId, string OrderName, int OrderOwnerId, string OrderOwnerUserName);

class GetAllOrdersQueryResultMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.Order.Order, GetAllOrdersQueryResult>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.OrderName, src => src.OrderName)
            .Map(dest => dest.OrderOwnerId, src => src.User.Id)
            .Map(dest => dest.OrderOwnerUserName, src => src.User.UserName);
    }
}