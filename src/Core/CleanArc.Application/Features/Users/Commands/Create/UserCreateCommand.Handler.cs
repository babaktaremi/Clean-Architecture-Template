using AutoMapper;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.Domain.Entities.User;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CleanArc.Application.Features.Users.Commands.Create;

internal class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, OperationResult<UserCreateCommandResult>>
{

    private readonly IAppUserManager _userManager;
    private readonly ILogger<UserCreateCommandHandler> _logger;
    private readonly IMapper _mapper;
    public UserCreateCommandHandler(IAppUserManager userRepository, ILogger<UserCreateCommandHandler> logger, IMapper mapper)
    {
        _userManager = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult<UserCreateCommandResult>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var userNameExist = await _userManager.IsExistUser(request.PhoneNumber);

        if (userNameExist)
            return OperationResult<UserCreateCommandResult>.FailureResult("Phone number already exists");

        var phoneNumberExist = await _userManager.IsExistUserName(request.UserName);

        if (phoneNumberExist)
            return OperationResult<UserCreateCommandResult>.FailureResult("Username already exists");

        //var user = new User { UserName = request.UserName, Name = request.FirstName, FamilyName = request.LastName, PhoneNumber = request.PhoneNumber };

        var user = _mapper.Map<User>(request);

        var createResult = await _userManager.CreateUser(user);

        if (!createResult.Succeeded)
        {
            return OperationResult<UserCreateCommandResult>.FailureResult(string.Join(",", createResult.Errors.Select(c => c.Description)));
        }

        var code = await _userManager.GeneratePhoneNumberConfirmationToken(user, user.PhoneNumber);


        _logger.LogWarning($"Generated Code for User ID {user.Id} is {code}");

        //TODO Send Code Via Sms Provider

        return OperationResult<UserCreateCommandResult>.SuccessResult(new UserCreateCommandResult { UserGeneratedKey = user.GeneratedCode });
    }
}