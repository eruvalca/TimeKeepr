using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Common.Logic;
using TimeKeepr.Application.Holidays.Dtos;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Enums;
using TimeKeepr.UI.Services;

namespace TimeKeepr.UI.Shared.Dashboard
{
    [Authorize]
    public partial class Dashboard
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private PtoEntriesClientService PtoEntriesService { get; set; }
        [Inject]
        private HolidaysClientService HolidaysService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private ApplicationUserDto User { get; set; }
        private List<PtoEntryDto> PtoEntries { get; set; }
        private List<HolidayDto> Holidays { get; set; }
        private decimal? VacationHoursAvailable { get; set; }
        public decimal? SickHoursAvailable { get; set; }
        public decimal? PersonalHoursAvailable { get; set; }
        private decimal? VacationHoursPlanned { get; set; }
        public decimal? SickHoursPlanned { get; set; }
        public decimal? PersonalHoursPlanned { get; set; }
        public decimal? VacationHoursCarriedOverRemaining { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var today = DateTime.Today;

            User = await IdentityService.GetUserDetails();
            var ptoEntriesRequest = await PtoEntriesService.GetByUserAndYear(User.Id, today.Year);
            var holidaysRequest = await HolidaysService.GetByYear(today.Year);

            if (ptoEntriesRequest.Succeeded && holidaysRequest.Succeeded)
            {
                PtoEntries = ptoEntriesRequest.Data.ToList();
                Holidays = holidaysRequest.Data.ToList();

                VacationHoursAvailable = PtoCalculator.GetVacationHoursAvailableByDate(User.HireDate, User.VacationDaysAccruedPerMonth, PtoEntries, today);
                SickHoursAvailable = PtoCalculator.GetSickHoursAvailableByDate(User.HireDate, User.SickHoursAccruedPerMonth, PtoEntries, today);
                PersonalHoursAvailable = PtoCalculator.GetPersonalHoursAvailableByDate(User.PersonalDaysPerYear, PtoEntries, today);

                VacationHoursPlanned = PtoCalculator.GetHoursPlannedAfterDateByType(PtoEntries, today, PtoType.Vacation) * -1;
                SickHoursPlanned = PtoCalculator.GetHoursPlannedAfterDateByType(PtoEntries, today, PtoType.Sick) * -1;
                PersonalHoursPlanned = PtoCalculator.GetHoursPlannedAfterDateByType(PtoEntries, today, PtoType.Personal) * -1;

                if (today < new DateTime(today.Year, 4, 1))
                {
                    VacationHoursCarriedOverRemaining = PtoCalculator.GetRemainingVacationHoursCarriedOverByYear(PtoEntries, today.Year) * -1;
                }
            }
        }
    }
}
