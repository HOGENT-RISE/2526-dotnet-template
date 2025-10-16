using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rise.Persistence;

namespace Rise.Server.Identity;

public class CustomClaimsPrincipalFactory(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor, ApplicationDbContext dbContext)
    : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        
        var technician = dbContext.Technicians.SingleOrDefaultAsync(x => x.AccountId == user.Id);
        
        // Add your custom claims
        identity.AddClaim(new Claim("TechnicianId", technician.Id.ToString()));
        identity.AddClaim(new Claim("CustomValue", "YourValueHere"));

        return identity;
    }
}