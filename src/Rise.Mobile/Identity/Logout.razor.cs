using Microsoft.AspNetCore.Components;

namespace Rise.Mobile.Identity;

public partial class Logout
{
    [Inject] public required IAccountManager AccountManager { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (await AccountManager.CheckAuthenticatedAsync())
        {
            await AccountManager.LogoutAsync();
        }
    }
}