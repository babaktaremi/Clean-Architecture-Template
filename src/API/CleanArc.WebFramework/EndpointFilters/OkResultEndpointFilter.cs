using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;

namespace CleanArc.WebFramework.EndpointFilters;

public class OkResultEndpointFilter:IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result=await next(context);

        if (result is not IStatusCodeHttpResult statusCodeResult)
        {
                return result;
        }

        if(statusCodeResult.StatusCode !=StatusCodes.Status200OK)
            return result;


        if (result is IValueHttpResult valueHttp)
        {
            return Results.Ok(new ApiResult<object>(true, ApiResultStatusCode.Success, valueHttp.Value));
        }

        return Results.Ok(new ApiResult(true, ApiResultStatusCode.Success)); 
    }
}