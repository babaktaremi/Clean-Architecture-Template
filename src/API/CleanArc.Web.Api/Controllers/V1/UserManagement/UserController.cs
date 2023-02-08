using CleanArc.Application.Features.Users.Commands.Create;
using CleanArc.Application.Features.Users.Commands.RefreshUserTokenCommand;
using CleanArc.Application.Features.Users.Commands.RequestLogout;
using CleanArc.Application.Features.Users.Queries.GenerateUserToken.Model;
using CleanArc.Application.Features.Users.Queries.TokenRequest;
using CleanArc.Web.Api.ApiModels.User;
using CleanArc.WebFramework.BaseController;
using CleanArc.WebFramework.Swagger;
using CleanArc.WebFramework.WebExtensions;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArc.Web.Api.Controllers.V1.UserManagement;

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

    [HttpPost("Register")]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        var command = await _mediator.Send(new UserCreateCommand(model.UserName, model.FirstName, model.LastName, model.PhoneNumber));

        return base.OperationResult(command);
    }

    //[HttpPost("ConfirmPhoneNumber")]
    //public async Task<IActionResult> ConfirmPhoneNumber(ConfirmUserPhoneNumberViewModel model)
    //{
    //    var command = await _mediator.Send(new ConfirmPhoneNumberCommand(model.UserKey, model.Code));

    //    return OperationResult(command);
    //}

    [HttpPost("TokenRequest")]
    public async Task<IActionResult> TokenRequest(UserTokenRequestViewModel model)
    {
        var query = await _mediator.Send(new UserTokenRequestQuery(model.UserPhoneNumber));

        return base.OperationResult(query);
    }

    [HttpPost("LoginConfirmation")]
    public async Task<IActionResult> ValidateUser(GenerateUserTokenViewModel model)
    {
        var result = await _mediator.Send(new GenerateUserTokenQuery(model.UserKey, model.Code));

        return base.OperationResult(result);
    }

    [HttpPost("RefreshSignIn")]
    [RequireTokenWithoutAuthorization]
    public async Task<IActionResult> RefreshUserToken(RefreshTokenViewModel model)
    {
        var checkCurrentAccessTokenValidity =await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

        if (checkCurrentAccessTokenValidity.Succeeded)
            return BadRequest("Current access token is valid. No need to refresh");

        var newTokenResult = await _mediator.Send(new RefreshUserTokenCommand(model.RefreshToken.ToString()));

        return base.OperationResult(newTokenResult);
    }

    [HttpPost("Logout")]
    [Authorize]
    public async Task<IActionResult> RequestLogout()
    {
        var commandResult = await _mediator.Send(new RequestLogoutCommandModel(base.UserId));

        return base.OperationResult(commandResult);
    }
}