using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Holidays.Dtos;
using TimeKeepr.UI.Services;

namespace TimeKeepr.UI.Pages.Holidays
{
    [Authorize]
    public partial class HolidaysList
    {
        [Inject]
        private HolidaysClientService HolidaysService { get;set; }

        [Parameter]
        public int Year { get; set; }

        private List<HolidayDto> Holidays { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var holidaysRequest = await HolidaysService.GetByYear(Year);

            if (holidaysRequest.Succeeded)
            {
                Holidays = holidaysRequest.Data.ToList();
            }
        }
    }
}
