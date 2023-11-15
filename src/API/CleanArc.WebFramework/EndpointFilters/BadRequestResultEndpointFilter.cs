using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;

namespace CleanArc.WebFramework.EndpointFilters;

public class BadRequestResultEndpointFilter:IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result is not IStatusCodeHttpResult statusCodeResult)
        {
            return result;
        }

        if (statusCodeResult.StatusCode != StatusCodes.Status400BadRequest)
            return result;


        if (result is IValueHttpResult valueHttp)
        {
            return Results.BadRequest(new ApiResult<object>(false, ApiResultStatusCode.BadRequest, valueHttp.Value));
        }

        return Results.BadRequest(new ApiResult(false, ApiResultStatusCode.BadRequest));
    }
}