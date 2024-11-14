using Asp.Versioning;
using CleanArc.Application.Features.Admin.Commands.AddAdminCommand;
using CleanArc.Application.Features.Admin.Queries.GetToken;
using CleanArc.Application.Models.Jwt;
using CleanArc.WebFramework.Attributes;
using CleanArc.WebFramework.BaseController;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArc.Web.Api.Controllers.V1.Admin
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/AdminManager")]
    public class AdminManagerController(ISender sender) : BaseController
    {
        [HttpPost("Login")]
        [ProducesOkApiResponseType<AccessToken>]
        public async Task<IActionResult> AdminLogin(AdminGetTokenQuery model)
        {
            var query = await sender.Send(model);

            return base.OperationResult(query);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("NewAdmin")]
        [ProducesOkApiResponseType]
        public async Task<IActionResult> AddNewAdmin(AddAdminCommand model)
        {
            var commandResult = await sender.Send(model);

            return base.OperationResult(commandResult);
        }
    }
}
