using Application.Models.ApiResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils;
using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace WebFramework.Filters
{
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                var controller = context.Controller as Controller;

                var model = context.ActionArguments.FirstOrDefault().Value;

                if (model != null)
                {
                    var errors = new ValidationProblemDetails(modelState);

                    var message = ApiResultStatusCode.BadRequest.ToDisplay();

                    var apiResult = new ApiResult<IDictionary<string, string[]>>(false, ApiResultStatusCode.BadRequest, errors.Errors, message);
                    context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status400BadRequest };
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }

                else
                {
                    var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                    context.Result = new JsonResult(apiResult) { StatusCode = 400};
                    context.HttpContext.Response.StatusCode = 400;
                }
                base.OnActionExecuting(context);
            }

           
        }
    }
}
