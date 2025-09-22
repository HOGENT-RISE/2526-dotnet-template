using Rise.Shared.Identity;
using Rise.Shared.Identity.Roles;

namespace Rise.Server.Endpoints.Identity.Roles;

using Microsoft.AspNetCore.Identity;

public class Create(RoleManager<IdentityRole> roleManager) : Endpoint<RoleRequest.Create, Result<string>>
{
    public override void Configure()
    {
        Post("/api/identity/roles");
        // Roles(AppRoles.Administrator);
    }

    public override async Task<Result<string>> ExecuteAsync(RoleRequest.Create req, CancellationToken ctx)
    {
        if(await roleManager.RoleExistsAsync(req.Name))
            return Result.Conflict($"Role with name '{req.Name}' already exists.");
        
        IdentityRole role = new()
        {
            Name = req.Name,
            NormalizedName = req.Name.ToUpper()
        };

        var result = await roleManager.CreateAsync(role);
        
        if(!result.Succeeded)
            return Result.Error(result.Errors.First().Description);
        
        return Result.Created(role.Id);
    }
}