using Microsoft.AspNetCore.Identity;

namespace Rise.Server.Endpoints.Identity.Account;

public class Logout(SignInManager<IdentityUser> signInManager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("api/identity/logout");
        AllowAnonymous();       
    }

    public override async Task<Result> HandleAsync(CancellationToken ct)
    {
        await signInManager.SignOutAsync();
        return Result.NoContent();       
    }
}