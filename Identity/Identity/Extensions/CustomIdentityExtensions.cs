using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArc.Infrastructure.Identity.Identity.Extensions;

public class DataProtectionTokenProviderOptions
{
    public string Name { get; set; } = "DataProtectorTokenProvider";
    public TimeSpan TokenLifespan { get; set; } 
}

public static class CustomIdentityExtensions
{
    public static IdentityBuilder AddPasswordlessLoginTotpTokenProvider(this IdentityBuilder builder)
    {
        var userType = builder.UserType;
        var totpProvider = typeof(PasswordlessLoginTotpTokenProvider<>).MakeGenericType(userType);
        return builder.AddTokenProvider("PasswordlessLoginTotpProvider", totpProvider);
    }
}

public class PasswordlessLoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser>
    where TUser : class
{
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(false);
    }

    public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var phone = await manager.GetPhoneNumberAsync(user);
        return "PasswordlessLogin:" + purpose + ":" + phone;
    }
}

public class PasswordlessLoginTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public PasswordlessLoginTokenProviderOptions()
    {
        // update the defaults
        Name = "PasswordlessLoginTokenProvider";
        TokenLifespan = TimeSpan.FromMinutes(1);
    }
}

public class PasswordlessLoginTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
    where TUser : class
{
    public PasswordlessLoginTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options,logger)
    {
    }
}