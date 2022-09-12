using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Identity.Dtos;
using Microsoft.AspNetCore.Components.Web;
using TimeKeepr.UI.Services;

namespace TimeKeepr.UI.Shared.Identity
{
    public partial class LoginDisplay
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private IdentityClientService IdentityService { get; set; }

        private ApplicationUserDto User { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var isAuthenticated = (await AuthenticationState).User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                User = await IdentityService.GetUserDetails();
            }

            StateHasChanged();
            await base.OnParametersSetAsync();
        }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await IdentityService.Logout();
            Navigation.NavigateTo("/");
        }
    }
}
