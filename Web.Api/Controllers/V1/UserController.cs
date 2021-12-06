using Application.Features.Admin.Queries.GetToken;
using Application.Features.Users.Commands.ConfirmPhoneNumber;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Queries.GenerateUserToken.Model;
using Application.Features.Users.Queries.TokenRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebFramework.BaseController;

namespace Web.Api.Controllers.V1
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/User")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreateCommand model)
        {
            var command = await _mediator.Send(model);

            if (command.IsSuccess)
            {
                return Ok(command.Result);
            }

            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("ConfirmPhoneNumber")]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmPhoneNumberCommand model)
        {
            var command = await _mediator.Send(model);

            if (command.IsSuccess)
            {
                return Ok(command.Result);
            }

            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("TokenRequest")]
        public async Task<IActionResult> TokenRequest(UserTokenRequestQuery model)
        {
            var query = await _mediator.Send(model);

            return base.OperationResult(query);
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> ValidateUser(GenerateUserTokenQuery model)
        {
            var result = await _mediator.Send(model);

            return base.OperationResult(result);
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin(AdminGetTokenQuery model)
        {
            var query = await _mediator.Send(model);

            return base.OperationResult(query);
        }
    }
}
