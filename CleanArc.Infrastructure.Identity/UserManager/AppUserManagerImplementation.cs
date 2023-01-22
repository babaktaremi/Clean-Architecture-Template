using CleanArc.Application.Contracts.Identity;
using CleanArc.Domain.Entities.User;
using CleanArc.Infrastructure.Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArc.Infrastructure.Identity.UserManager;

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

    public async Task<string> GeneratePhoneNumberConfirmationToken(User user, string phoneNumber)
    {
        return await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
    }

    public Task<User> GetUserByCode(string code)
    {
        return _userManager.Users.FirstOrDefaultAsync(c => c.GeneratedCode.Equals(code));
    }

    public async Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string code)
    {
        return await _userManager.ChangePhoneNumberAsync(user, phoneNumber, code);
    }

    public async Task<IdentityResult> VerifyUserCode(User user, string code)
    {
        var confirmationResult=await _userManager.VerifyUserTokenAsync(
            user, "PasswordlessLoginTotpProvider", "passwordless-auth", code);


        return confirmationResult
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError() { Description = "Incorrect Code" });
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

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userManager.Users.AsNoTracking().ToListAsync();
    }

    public async Task<IdentityResult> CreateUserWithPasswordAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(User user, Role role)
    {
        ArgumentNullException.ThrowIfNull(role,nameof(role));
        ArgumentNullException.ThrowIfNull(role.Name,nameof(role.Name));

        return await _userManager.AddToRoleAsync(user, role.Name);
    }
}