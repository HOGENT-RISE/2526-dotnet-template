using Microsoft.AspNetCore.Identity;
using Rise.Domain.Products;
using Rise.Persistence;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

public class Register(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore) : Endpoint<AccountRequest.Register, Result>
{
    public override void Configure()
    {
        Post("/api/identity/accounts/register");
        AllowAnonymous();
    }

    public override async Task<Result> ExecuteAsync(AccountRequest.Register req, CancellationToken ctx)
    {
        if (!userManager.SupportsUserEmail)
        {
            return Result.CriticalError("Requires a user store with email support.");
        }
        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var user = new IdentityUser();
        await userStore.SetUserNameAsync(user, req.Email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, req.Email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, req.Password);
        
        if (!result.Succeeded)
        {
            return Result.Error(result.Errors.First().Description);
        }
        
        // You can do more stuff when injecting a DbContext and create user stuff for example:
        // dbContext.Products.Add(new Product("MyName", "MyDescription"));
        // or assinging a specific role etc using the RoleManager<IdentityUser> (also injected)

        
        // You can send a confirmation email by using a SMTP server or anything in the like. 
        //await SendConfirmationEmailAsync(user, userManager, context, email); or do something that matters

        return Result.Success();
    }
    
}