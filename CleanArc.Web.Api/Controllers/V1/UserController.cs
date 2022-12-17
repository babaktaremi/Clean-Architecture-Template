using CleanArc.Application.Features.Admin.Queries.GetToken;
using CleanArc.Application.Features.Users.Commands.ConfirmPhoneNumber;
using CleanArc.Application.Features.Users.Commands.Create;
using CleanArc.Application.Features.Users.Queries.GenerateUserToken.Model;
using CleanArc.Application.Features.Users.Queries.TokenRequest;
using CleanArc.Web.Api.ApiModels.User;
using CleanArc.WebFramework.BaseController;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArc.Web.Api.Controllers.V1;

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
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        var command = await _mediator.Send(new UserCreateCommand(model.UserName,model.FirstName,model.LastName,model.PhoneNumber));

        return base.OperationResult(command);
    }

    [HttpPost("ConfirmPhoneNumber")]
    public async Task<IActionResult> ConfirmPhoneNumber(ConfirmUserPhoneNumberViewModel model)
    {
        var command = await _mediator.Send(new ConfirmPhoneNumberCommand(model.UserKey,model.Code));

        return base.OperationResult(command);
    }

    [HttpPost("TokenRequest")]
    public async Task<IActionResult> TokenRequest(UserTokenRequestViewModel model)
    {
        var query = await _mediator.Send(new UserTokenRequestQuery(model.UserPhoneNumber));

        return base.OperationResult(query);
    }

    [HttpPost("GetToken")]
    public async Task<IActionResult> ValidateUser(GenerateUserTokenViewModel model)
    {
        var result = await _mediator.Send(new GenerateUserTokenQuery(model.UserKey,model.Code));

        return base.OperationResult(result);
    }

    [HttpPost("AdminLogin")]
    public async Task<IActionResult> AdminLogin(AdminTokenRequestViewModel model)
    {
        var query = await _mediator.Send(new AdminGetTokenQuery(model.UserName,model.Password));

        return base.OperationResult(query);
    }
}