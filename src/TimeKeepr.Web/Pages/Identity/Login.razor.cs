using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.UI.Services;

namespace TimeKeepr.Web.Pages.Identity
{
    public partial class Login
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private LoginDto LoginDto { get; set; } = new LoginDto();

        private List<string> ServerMessages { get; set; } = new List<string>();

        private bool ShowServerErrors { get; set; } = false;
        private bool DisableSubmit { get; set; } = false;

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;

            var response = await IdentityService.Login(LoginDto);

            if (response.Succeeded)
            {
                Navigation.NavigateTo("/");
            }
            else
            {
                ServerMessages = response.Errors.ToList();
                ShowServerErrors = true;
                DisableSubmit = false;
            }
        }
    }
}
