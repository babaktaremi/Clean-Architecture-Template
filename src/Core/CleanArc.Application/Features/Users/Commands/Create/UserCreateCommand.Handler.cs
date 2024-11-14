using AutoMapper;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Models.Common;
using CleanArc.Domain.Entities.User;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CleanArc.Application.Features.Users.Commands.Create;

internal class UserCreateCommandHandler(
    IAppUserManager userRepository,
    ILogger<UserCreateCommandHandler> logger,
    IMapper mapper)
    : IRequestHandler<UserCreateCommand, OperationResult<UserCreateCommandResult>>
{
    public async ValueTask<OperationResult<UserCreateCommandResult>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var userNameExist = await userRepository.IsExistUser(request.PhoneNumber);

        if (userNameExist)
            return OperationResult<UserCreateCommandResult>.FailureResult("Phone number already exists");

        var phoneNumberExist = await userRepository.IsExistUserName(request.UserName);

        if (phoneNumberExist)
            return OperationResult<UserCreateCommandResult>.FailureResult("Username already exists");

        //var user = new User { UserName = request.UserName, Name = request.FirstName, FamilyName = request.LastName, PhoneNumber = request.PhoneNumber };

        var user = mapper.Map<User>(request);

        var createResult = await userRepository.CreateUser(user);

        if (!createResult.Succeeded)
        {
            return OperationResult<UserCreateCommandResult>.FailureResult(string.Join(",", createResult.Errors.Select(c => c.Description)));
        }

        var code = await userRepository.GeneratePhoneNumberConfirmationToken(user, user.PhoneNumber);


        logger.LogWarning($"Generated Code for User ID {user.Id} is {code}");

        //TODO Send Code Via Sms Provider

        return OperationResult<UserCreateCommandResult>.SuccessResult(new UserCreateCommandResult { UserGeneratedKey = user.GeneratedCode });
    }
}