using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArc.WebFramework.Filters;

public class NotFoundResultAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if ((context.Result is NotFoundObjectResult notFoundObjectResult))
        {
            var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
        }

        else if(context.Result is NotFoundResult)
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
            context.Result = new JsonResult(apiResult) { StatusCode =StatusCodes.Status404NotFound };
        }
    }
}