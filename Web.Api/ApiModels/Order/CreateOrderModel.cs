using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.Order;

public record CreateOrderModel([Required(ErrorMessage = "Please Enter Your Order Name")]
    string OrderName);