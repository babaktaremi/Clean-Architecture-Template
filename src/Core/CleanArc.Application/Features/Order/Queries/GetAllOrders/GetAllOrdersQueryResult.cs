using AutoMapper;
using CleanArc.Application.Profiles;

namespace CleanArc.Application.Features.Order.Queries.GetAllOrders;

public record GetAllOrdersQueryResult(int OrderId, string OrderName, int OrderOwnerId, string OrderOwnerUserName)
    : ICreateMapper<Domain.Entities.Order.Order>
{
    public void Map(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Order.Order,GetAllOrdersQueryResult>()
            .ForCtorParam(nameof(OrderId),opt=>opt.MapFrom(c=>c.Id))
            .ForCtorParam(nameof(OrderOwnerId),opt=>opt.MapFrom(c=>c.UserId))
            .ForCtorParam(nameof(OrderOwnerUserName),opt=>opt.MapFrom(c=>c.User.UserName))
            .ReverseMap();
        
    }
}