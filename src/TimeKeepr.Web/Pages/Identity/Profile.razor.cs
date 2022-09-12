using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.UI.Services;

namespace TimeKeepr.Web.Pages.Identity
{
    [Authorize]
    public partial class Profile
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private PtoEntriesClientService PtoEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private ApplicationUserDto User { get; set; }
        private UpdateApplicationUserDto UserDto { get; set; }

        private decimal _vacationCarriedOver { get; set; }
        private decimal VacationCarriedOver
        {
            get { return _vacationCarriedOver; }
            set
            {
                _vacationCarriedOver = value;

                if (_vacationCarriedOver > 5)
                {
                    _vacationCarriedOver = 5;
                }

                if (_vacationCarriedOver < 0)
                {
                    _vacationCarriedOver = 0;
                }
            }
        }

        private List<string> ServerMessages { get; set; } = new List<string>();
        private bool ShowServerErrors { get; set; } = false;
        private bool DisableSubmit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            User = await IdentityService.GetUserDetails();
            //var vacationCarriedOverEntry = await PTOEntriesService.GetVacationCarriedOverEntryByUserId(TimeKeepUser.Id);

            //VacationCarriedOver = vacationCarriedOverEntry is null ? 0 : vacationCarriedOverEntry.PTOHours / 8;

            UserDto = new UpdateApplicationUserDto()
            {
                Id = User.Id,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                HireDate = User.HireDate,
                VacationDaysAccruedPerMonth = User.VacationDaysAccruedPerMonth,
                SickHoursAccruedPerMonth = User.SickHoursAccruedPerMonth,
                PersonalDaysPerYear = User.PersonalDaysPerYear
            };
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;

            //User.VacationDaysCarriedOver = VacationCarriedOver;

            var response = await IdentityService.UpdateUser(UserDto);

            if (response.Succeeded)
            {
                await IdentityService.Logout();
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
