namespace Rise.Shared.Identity.Accounts;

/// <summary>
/// Represents a request structure for account-related operations, such as registration or authentication.
/// </summary>
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

        /// <summary>
        /// Provides validation rules for the Register class fields such as email and password.
        /// </summary>
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