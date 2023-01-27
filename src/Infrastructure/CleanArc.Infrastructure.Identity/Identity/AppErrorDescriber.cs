using Microsoft.AspNetCore.Identity;

namespace CleanArc.Infrastructure.Identity.Identity;

public class AppErrorDescriber:IdentityErrorDescriber
{
    public override IdentityError DefaultError()
    {
        return new IdentityError
        {
            Code = "DefaultError",
            Description = "There was an error"
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = "Specified email already exists"
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = "specified username already exists"
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = "Incorrect password"
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = "Invalid password. Password is to short"
        };
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = "Invalid username"
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = "Invalid email"
        };
    }

    public override IdentityError InvalidToken()
    {
            
        return new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = "Invalid given code. Please try again"
        };
    }

}