using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Web.Shared.PtoEntries
{
    [Authorize]
    public partial class PtoEntriesTable
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public List<PtoEntryDto> PtoEntries { get; set; }
    }
}
