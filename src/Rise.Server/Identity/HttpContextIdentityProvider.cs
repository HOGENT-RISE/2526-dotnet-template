using System.Security.Claims;
using Rise.Services.Identity;

namespace Rise.Server.Identity;

public class HttpContextSessionProvider(IHttpContextAccessor httpContextAccessor) : ISessionContextProvider 
{ 
    public ClaimsPrincipal? User => httpContextAccessor!.HttpContext?.User; 
} 
