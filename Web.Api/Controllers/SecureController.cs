using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
