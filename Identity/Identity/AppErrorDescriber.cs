using Microsoft.AspNetCore.Identity;

namespace Identity.Identity
{
    public class AppErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = "DefaultError",
                Description = "خطایی رخ داده است"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "ایمیل وارد شده تکراری است"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "نام کاربری وارد شده تکراری است"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "پسوورد وارد شده صحیح نیست"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = "پسوورد کوتاه است"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "نام کاربری نامعتبر است"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "ایمیل وارد شده نامعتبر است"
            };
        }

        public override IdentityError InvalidToken()
        {
            
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "کد وارد شده معتبر نیست"
            };
        }

    }
}
