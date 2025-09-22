using System.Security.Claims;

namespace Rise.Services.Identity;

public interface ISessionContextProvider
{
    ClaimsPrincipal? User { get; } 
}