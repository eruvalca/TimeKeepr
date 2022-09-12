using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace TimeKeepr.Web.Shared
{
    [Authorize]
    public partial class AvailableHoursDisplay
    {
        [Parameter]
        public decimal VacationHoursAvailable { get; set; }
        [Parameter]
        public decimal SickHoursAvailable { get; set; }
        [Parameter]
        public decimal PersonalHoursAvailable { get; set; }
    }
}
