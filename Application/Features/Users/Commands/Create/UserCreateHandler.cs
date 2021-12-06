using System.Diagnostics;
using Application.Contracts.Identity;
using Application.Models.Common;
using Domain.Entities.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.Create
{
    public class UserCreateHandler : IRequestHandler<UserCreateCommand, OperationResult<UserCreateCommandResult>>
    {

        private readonly IAppUserManager _userRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<UserCreateHandler> _logger;
        public UserCreateHandler(IAppUserManager userRepository, IMediator mediator, ILogger<UserCreateHandler> logger)
        {
            _userRepository = userRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<OperationResult<UserCreateCommandResult>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var userNameExist = await _userRepository.IsExistUser(request.PhoneNumber);

            if (userNameExist)
                return OperationResult<UserCreateCommandResult>.FailureResult("این شماره تلفن وجود دارد");

            var phoneNumberExist = await _userRepository.IsExistUserName(request.UserName);

            if (phoneNumberExist)
                return OperationResult<UserCreateCommandResult>.FailureResult("این نام کاربری دارد");

            var user = new User { UserName = request.UserName, Name = request.FirstName, FamilyName = request.LastName, PhoneNumber = request.PhoneNumber, PhoneNumberConfirmed = true };

            var createResult = await _userRepository.CreateUser(user);

            if (!createResult.Succeeded)
            {
                return OperationResult<UserCreateCommandResult>.FailureResult(string.Join(",", createResult.Errors.Select(c => c.Description)));
            }

            var code = await _userRepository.GeneratePhoneNumberToken(user, user.PhoneNumber);


            _logger.LogWarning($"Generated Code for User ID {user.Id} is {code}");

            //TODO Send Code Via Sms Provider

            return OperationResult<UserCreateCommandResult>.SuccessResult(new UserCreateCommandResult { UserGeneratedKey = user.GeneratedCode });
        }
    }
}
