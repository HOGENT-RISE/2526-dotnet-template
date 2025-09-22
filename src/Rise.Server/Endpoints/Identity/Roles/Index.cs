using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Rise.Shared.Identity;
using Rise.Shared.Identity.Roles;

namespace Rise.Server.Endpoints.Identity.Roles;

using Microsoft.AspNetCore.Identity;

public class Index(RoleManager<IdentityRole> roleManager) : EndpointWithoutRequest<Result<List<KeyValuePair<string, string>>>>
{
    public override void Configure()
    {
        Get("/api/identity/roles");
        // Roles(AppRoles.Administrator);
    }

    public override async Task<Result<List<KeyValuePair<string, string>>>> ExecuteAsync(CancellationToken ctx)
    {
        var roles = await roleManager.Roles.Select(r => new KeyValuePair<string, string>(r.Id, r.Name!)).ToListAsync(ctx);
        return Result.Success(roles);
    }
}