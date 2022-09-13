using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.UI.Services;

namespace TimeKeepr.UI.Pages.Identity
{
    public partial class Register
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private PtoEntriesClientService PtoEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private RegisterDto RegisterDto { get; set; } = new RegisterDto();
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

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;

            var response = await IdentityService.Register(RegisterDto);

            if (response.Succeeded)
            {
                if (VacationCarriedOver > 0)
                {
                    var user = await IdentityService.GetUserDetails();
                    var vacationCarryOverEntry = new CreatePtoEntryDto()
                    {
                        CreatedBy = user.Id,
                        PtoHours = VacationCarriedOver,
                        PtoDate = new DateTime(DateTime.Today.Year, 1, 1),
                        PtoType = Domain.Enums.PtoType.VacationCarryOver
                    };

                    _ = await PtoEntriesService.Create(vacationCarryOverEntry);
                }

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
