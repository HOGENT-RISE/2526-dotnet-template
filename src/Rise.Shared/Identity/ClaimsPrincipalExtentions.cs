using System.Security.Claims;

namespace Rise.Shared.Identity;

public static class ClaimsPrincipalExtentions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserName(this ClaimsPrincipal user) =>
        user?.FindFirst(ClaimTypes.Name)?.Value;

    public static string? GetEmail(this ClaimsPrincipal user) =>
        user?.FindFirst(ClaimTypes.Email)?.Value;

    public static bool IsInRole(this ClaimsPrincipal user, string role) =>
        user?.IsInRole(role) ?? false;
}