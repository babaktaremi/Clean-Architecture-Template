using System.Security.Claims;
using Application.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utils;
using WebFramework.Filters;

namespace WebFramework.BaseController
{
    public class BaseController : ControllerBase
    {
        protected string UserName => User.Identity.Name;
        protected int UserId => int.Parse(User.Identity.GetUserId());
        protected string UserEmail => User.Identity.FindFirstValue(ClaimTypes.Email);
        protected string UserRole => User.Identity.FindFirstValue(ClaimTypes.Role);

        protected string UserKey => User.FindFirstValue(ClaimTypes.UserData);

        //public UserRepository UserRepository { get; set; } => property injection
        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected IActionResult OperationResult(dynamic result)
        {
            if (result is null)
                return new ServerErrorResult("مشکلی به وجود آمده است");

            if (!((object)result).IsAssignableFromBaseTypeGeneric(typeof(OperationResult<>)))
            {
                throw new InvalidCastException("Given Type is not an OperationResult<T>");
            }


            if (result.IsSuccess) return result.Result is bool ? Ok() : Ok(result.Result);

            if (result.IsNotFound)
                return NotFound(result.ErrorMessage);

            ModelState.AddModelError("GeneralError", result.ErrorMessage);
            return BadRequest(ModelState);

        }
    }
}
