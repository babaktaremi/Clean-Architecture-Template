using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebFrameWork.Filters
{
   public class ServerErrorResult:IActionResult
    {
        public string Message { get;}

        public ServerErrorResult(string message)
        {
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new ApiResult(false, ApiResultStatusCode.ServerError, Message);
           await context.HttpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
