using CleanArc.Domain.Entities.User;
using CleanArc.Tests.Setup.Setups;

namespace CleanArc.Test.Infrastructure.Identity
{
    public class UserManagerTest: TestIdentitySetup
    {
        [Fact]
        public async Task Duplicate_User_Names_Not_Allowed()
        {
            var user = new User()
            {
                UserName = "Test", Email = "Test@example.com"
            };

            var duplicateUser = new User()
            {
                UserName = "Test",
                Email = "Test@example.com"
            };

            var createUserResult = await base.TestAppUserManager.CreateUser(user);

            var duplicateCreateUserResult = await base.TestAppUserManager.CreateUser(duplicateUser);

            Assert.False(duplicateCreateUserResult.Succeeded);
        }

        [Fact]
        public async Task Create_Phone_Confirmation_Code_And_Verify()
        {
            var user = new User()
            {
                UserName = "Test", 
                Email = "test@example.com",
                PhoneNumber = "09123456789"
            };

            var createUserResult = await base.TestAppUserManager.CreateUser(user);

            var otpCode=await base.TestAppUserManager.GeneratePhoneNumberConfirmationToken(user,user.PhoneNumber);

            var confirmPhoneNumberResult=await base.TestAppUserManager.ChangePhoneNumber(user,user.PhoneNumber,otpCode);


            Assert.NotNull(otpCode);
            Assert.True(confirmPhoneNumberResult.Succeeded);
            Assert.True(user.PhoneNumberConfirmed);
        }

        [Fact]
        public async Task Generate_And_Verify_Otp_Code()
        {
            var user = new User()
            {
                UserName = "Test",
                Email = "test@example.com",
                PhoneNumber = "09123456789"
            };

            var createUserResult = await base.TestAppUserManager.CreateUser(user);

            var phoneNumberConfirmationCode = await base.TestAppUserManager.GeneratePhoneNumberConfirmationToken(user, user.PhoneNumber);

            var confirmPhoneNumberResult = await base.TestAppUserManager.ChangePhoneNumber(user, user.PhoneNumber, phoneNumberConfirmationCode);

            var otpCode = await base.TestAppUserManager.GenerateOtpCode(user);

            var confirmOtpCode = await base.TestAppUserManager.VerifyUserCode(user, otpCode);

            Assert.NotNull(otpCode);
            Assert.True(confirmOtpCode.Succeeded);
        }

        [Fact]
        public async Task Generate_Access_Token()
        {
            var user = new User()
            {
                UserName = "Test",
                Email = "test@example.com",
                PhoneNumber = "09123456789"
            };

            var createUserResult = await base.TestAppUserManager.CreateUser(user);

            var otpCode = await base.TestAppUserManager.GeneratePhoneNumberConfirmationToken(user, user.PhoneNumber);

            var confirmPhoneNumberResult = await base.TestAppUserManager.ChangePhoneNumber(user, user.PhoneNumber, otpCode);

            var token=await base.JwtService.GenerateAsync(user);

            Assert.NotNull(token.access_token);
        }
    }
}