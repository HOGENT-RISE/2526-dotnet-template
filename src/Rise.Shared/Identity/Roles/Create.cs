namespace Rise.Shared.Identity.Roles;

public static partial class RoleRequest
{
    public class Create
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        public required string Name { get; init; }

        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
    }
}