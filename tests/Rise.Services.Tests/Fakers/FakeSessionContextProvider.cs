using System.Security.Claims;
using Rise.Services.Identity;

namespace Rise.Services.Tests.Fakers;

public class FakeSessionContextProvider : ISessionContextProvider
{
    public FakeSessionContextProvider(ClaimsPrincipal user) => User = user;
    public ClaimsPrincipal? User { get; }
    public int? TechnicianId
    {
        get
        {
            var value = User?.FindFirst("TechnicianId")?.Value;
            return int.TryParse(value, out var id) ? id : null;
        }
    }
}