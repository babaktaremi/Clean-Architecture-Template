namespace CleanArc.Application.Features.Order.Queries.GetAllOrders;

public record GetAllOrdersQueryResult(int OrderId,string OrderName,int OrderOwnerId,string OrderOwnerUserName);