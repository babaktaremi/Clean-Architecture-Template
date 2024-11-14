using System.Security.Claims;
using CleanArc.Application.Models.Common;
using CleanArc.SharedKernel.Extensions;
using CleanArc.WebFramework.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArc.WebFramework.BaseController;

public class BaseController : ControllerBase
{
    protected string UserName => User.Identity?.Name;
    protected int UserId => int.Parse(User.Identity.GetUserId());
    protected string UserEmail => User.Identity.FindFirstValue(ClaimTypes.Email);
    protected string UserRole => User.Identity.FindFirstValue(ClaimTypes.Role);

    protected string UserKey => User.FindFirstValue(ClaimTypes.UserData);

    protected IActionResult OperationResult<TModel>(OperationResult<TModel> result)
    {
        if (result is null)
            return new ServerErrorResult("Server Error");


        if (result.IsSuccess)
            return result.Result is bool ? Ok() : Ok(result.Result);

        if (result.IsNotFound)
        {

            AddErrors(result);

            var notFoundErrors = new ValidationProblemDetails(ModelState);

            return NotFound(notFoundErrors.Errors);
        }

        AddErrors(result);
        
        var badRequestErrors = new ValidationProblemDetails(ModelState);

        return BadRequest(badRequestErrors.Errors);

    }
    
    private void AddErrors<TModel>(OperationResult<TModel> result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key,error.Value);
        }
    }
}