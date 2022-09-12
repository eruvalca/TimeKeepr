using Microsoft.AspNetCore.Components;

namespace TimeKeepr.UI.Shared.Identity
{
    public partial class RedirectToLogin
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo("/user/login");
        }
    }
}
