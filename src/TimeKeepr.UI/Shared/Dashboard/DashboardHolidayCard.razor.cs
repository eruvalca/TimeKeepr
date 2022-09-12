using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Holidays.Dtos;

namespace TimeKeepr.UI.Shared.Dashboard
{
    [Authorize]
    public partial class DashboardHolidayCard
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public List<HolidayDto> Holidays { get; set; }
        [Parameter]
        public DateTime Date { get; set; }

        private HolidayDto NextHolidayAfterDate { get; set; }
        private int RemainingHolidays { get; set; }

        protected override void OnParametersSet()
        {
            NextHolidayAfterDate = Holidays.Where(h => h.Date.Date >= Date.Date).OrderBy(h => h.Date).FirstOrDefault();
            RemainingHolidays = Holidays.Where(h => h.Date.Date >= Date.Date).Count();
        }
    }
}
