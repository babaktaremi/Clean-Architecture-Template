using Microsoft.AspNetCore.Mvc;
using Identity.Identity.PermissionManager;
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
        [HttpGet("Secure")]
        public IActionResult Secure()
        {
            return Ok();
        }
    }
}
