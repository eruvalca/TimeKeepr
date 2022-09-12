using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace TimeKeepr.Web.Shared.Dashboard
{
    [Authorize]
    public partial class DashboardCard
    {
        [Parameter]
        public string PtoType { get; set; }
        [Parameter]
        public decimal HoursAvailable { get; set; }
        [Parameter]
        public decimal HoursPlanned { get; set; }
        [Parameter]
        public decimal? HoursExpiring { get; set; }
    }
}
