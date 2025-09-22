namespace Rise.Shared.Identity.Accounts;
public static partial  class AccountResponse
{
    public class Info
    {
        public required string Email { get; set; }
        public required bool IsEmailConfirmed { get; set; }
        public required Dictionary<string, string> Claims { get; set; } = [];
        public List<string> Roles { get; set; } = [];
    }
}