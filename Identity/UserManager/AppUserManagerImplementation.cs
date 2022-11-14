using Application.Contracts.Identity;
using Domain.Entities.User;
using Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.UserManager
{
   public class AppUserManagerImplementation:IAppUserManager
   {
       private readonly AppUserManager _userManager;
       private readonly AppSignInManager _signInManager;
       public AppUserManagerImplementation(AppUserManager userManager, AppSignInManager signInManager)
       {
           _userManager = userManager;
           _signInManager = signInManager;
       }

        public Task<IdentityResult> CreateUser(User user)
        {
            return _userManager.CreateAsync(user);
        }

        public Task<bool> IsExistUser(string phoneNumber)
        {
            return _userManager.Users.AnyAsync(c => c.PhoneNumber == phoneNumber);
        }

        public Task<bool> IsExistUserName(string userName)
        {
            return _userManager.Users.AnyAsync(c => c.UserName.Equals(userName));
        }

        public Task<string> GeneratePhoneNumberToken(User user, string phoneNumber)
        {
            return _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        }

        public Task<User> GetUserByCode(string code)
        {
            return _userManager.Users.FirstOrDefaultAsync(c => c.GeneratedCode.Equals(code));
        }

        public Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string code)
        {
            return _userManager.ChangePhoneNumberAsync(user, phoneNumber, code);
        }

        public Task<bool> VerifyUserCode(User user, string code)
        {
            return _userManager.VerifyUserTokenAsync(
                user, "PasswordlessLoginTotpProvider", "passwordless-auth", code);
        }

        public Task<string> GenerateOtpCode(User user)
        {
           return _userManager.GenerateUserTokenAsync(
                user, "PasswordlessLoginTotpProvider", "passwordless-auth");
        }

        public Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber.Equals(phoneNumber));
        }

        public Task<SignInResult> AdminLogin(User user, string password)
        {
            return _signInManager.PasswordSignInAsync(user, password, true, true);
        }

        public Task<User> GetByUserName(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.AsNoTracking().ToListAsync();
        }
   }
}
