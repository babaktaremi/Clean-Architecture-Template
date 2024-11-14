using CleanArc.Application.Contracts;
using CleanArc.Application.Contracts.Identity;
using CleanArc.Application.Contracts.Persistence;
using CleanArc.Domain.Entities.User;
using CleanArc.Infrastructure.Identity.Identity.Dtos;
using CleanArc.Infrastructure.Identity.Identity.Extensions;
using CleanArc.Infrastructure.Identity.Identity.Manager;
using CleanArc.Infrastructure.Identity.Identity.Store;
using CleanArc.Infrastructure.Identity.Jwt;
using CleanArc.Infrastructure.Identity.UserManager;
using CleanArc.Infrastructure.Persistence;
using CleanArc.Infrastructure.Persistence.Repositories.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArc.Tests.Setup.Setups;

public abstract class TestIdentitySetup
{
    protected IAppUserManager TestAppUserManager { get; }
    protected AppRoleManager TestAppRoleManager { get; }
    public AppSignInManager TestSignInManager { get; }
    public IAppUserManager User { get; }
    public IJwtService JwtService { get; }

    protected TestIdentitySetup()
    {
        var serviceCollection = new ServiceCollection();

        var connection = new SqliteConnection("DataSource=:memory:");

        serviceCollection.AddLogging();

        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));

        var context = serviceCollection.BuildServiceProvider().GetService<ApplicationDbContext>();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();


        serviceCollection.AddIdentity<User, Role>(options =>
        {
            options.Stores.ProtectPersonalData = false;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = true;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;
            options.User.RequireUniqueEmail = false;

        }).AddUserStore<AppUserStore>()
            .AddRoleStore<RoleStore>().
            AddUserManager<AppUserManager>().
            AddRoleManager<AppRoleManager>().
            AddDefaultTokenProviders().
            AddSignInManager<AppSignInManager>()
            .AddDefaultTokenProviders()
            .AddPasswordlessLoginTotpTokenProvider();

        serviceCollection.Configure<IdentitySettings>(settings =>
        {
            settings.Audience = "CleanArc.Unit.Tests";
            settings.ExpirationMinutes = 5;
            settings.Issuer = "CleanArc.Unit.Tests";
            settings.NotBeforeMinutes = 0;
            settings.SecretKey = "ShouldBe-LongerThan-16Char-SecretKey";
            settings.Encryptkey = "16CharEncryptKey";
        });

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IJwtService, JwtService>();
        serviceCollection.AddScoped<IAppUserManager, AppUserManagerImplementation>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        TestAppUserManager = serviceProvider.GetRequiredService<IAppUserManager>();
        TestAppRoleManager = serviceProvider.GetRequiredService<AppRoleManager>();
        TestSignInManager = serviceProvider.GetRequiredService<AppSignInManager>();
        JwtService = serviceProvider.GetRequiredService<IJwtService>();
    }
}
