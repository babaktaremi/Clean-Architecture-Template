using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArc.WebFramework.Filters;

[Obsolete(message:"Separated filters added")]
public class ApiResultFilterAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is OkObjectResult okObjectResult)
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
        }
        else if (context.Result is OkResult okResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
            context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
        }
        else if (context.Result is BadRequestResult badRequestResult)
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
            context.Result = new JsonResult(apiResult) { StatusCode = badRequestResult.StatusCode };
            context.HttpContext.Response.StatusCode = 400;
        }
        else if (context.Result is BadRequestObjectResult badRequestObjectResult)
        {
            var message = badRequestObjectResult.Value.ToString();
            if (badRequestObjectResult.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
            context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
            context.HttpContext.Response.StatusCode = 400;
        }
        else if (context.Result is ContentResult contentResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
            context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
        }
        else if (context.Result is NotFoundResultAttribute notFoundResult)
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
            context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status404NotFound };
        }
        else if (context.Result is NotFoundObjectResult notFoundObjectResult)
        {
            var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
        }
        else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null 
                                                             && !(objectResult.Value is ApiResult))
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
            context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
        }

        base.OnResultExecuting(context);
    }
}