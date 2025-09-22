namespace Rise.Shared.Identity.Accounts;

public static partial class AccountRequest
{
    public class Register
    {
        /// <summary>
        /// The user's email address which acts as a user name.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; init; }
        
        // Other needed stuff here, like Role(s), Firstname, lastname etc.
        
        public class Validator : AbstractValidator<Register>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
    }
}