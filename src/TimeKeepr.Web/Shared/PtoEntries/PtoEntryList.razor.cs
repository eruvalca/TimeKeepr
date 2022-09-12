using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Entities;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Web.Shared.PtoEntries
{
    [Authorize]
    public partial class PtoEntryList
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public List<PtoEntryDto> PtoEntries { get; set; }

        protected override void OnParametersSet()
        {
            PtoEntries = PtoEntries
                .Where(p => p.PtoType != PtoType.VacationCarryOver)
                .Select(p => p)
                .OrderBy(p => p.PtoDate)
                .ThenBy(p => p.PtoEntryId)
                .ToList();
        }
    }
}
