using Application.Features.Users.Queries.GetUsers.Model;
using Microsoft.AspNetCore.Mvc;
using Identity.Identity.PermissionManager;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebFramework.BaseController;

namespace Web.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/Secure")]
    [Authorize(policy: nameof(ConstantPolicies.DynamicPermission))]
    public class SecureController : BaseController
    {
        private readonly ISender _sender;

        public SecureController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("Secure")]
        public IActionResult Secure()
        {
            return Ok();
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _sender.Send(new GetUsersQueryModel());

            return base.OperationResult(result);
        }
    }
}
