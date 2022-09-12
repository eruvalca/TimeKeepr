using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeepr.Application.Common.Logic;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Enums;
using TimeKeepr.Web.Services;

namespace TimeKeepr.Web.Pages.PtoEntry
{
    [Authorize]
    public partial class EditPtoEntry
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private PtoEntriesClientService PtoEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public int Id { get; set; }

        private ApplicationUserDto? User { get; set; }
        private UpdatePtoEntryDto PtoDto { get; set; } = new();
        private IEnumerable<PtoEntryDto>? UserPtoEntries { get; set; }

        private decimal _ptoHours;
        public decimal PtoHours
        {
            get { return _ptoHours; }
            set
            {
                _ptoHours = value;

                _ptoHours = Math.Floor(_ptoHours);

                if (_ptoHours > 8)
                {
                    _ptoHours = 8;
                }

                if (_ptoHours > SelectedHoursAvailable)
                {
                    _ptoHours = SelectedHoursAvailable;
                }

                if (_ptoHours < 0)
                {
                    _ptoHours = 0;
                }
            }
        }
        private PtoType _ptoType;
        public PtoType PtoType
        {
            get { return _ptoType; }
            set
            {
                _ptoType = value;
                UpdateSelectedHoursAvailable(_ptoType);
                PtoHours = _ptoHours;
            }
        }
        private DateTime _ptoDate;
        public DateTime PtoDate
        {
            get { return _ptoDate; }
            set
            {
                _ptoDate = value;
                GetAvailablePTOHoursByDate(_ptoDate);
            }
        }

        public decimal? VacationHoursAvailable { get; set; }
        public decimal? SickHoursAvailable { get; set; }
        public decimal? PersonalHoursAvailable { get; set; }
        public decimal SelectedHoursAvailable { get; set; }
        private List<string> ServerMessages { get; set; } = new List<string>();
        private bool ShowServerErrors { get; set; } = false;
        private bool DisableSubmit { get; set; } = false;
        private bool DisableDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            User = await IdentityService.GetUserDetails();

            var ptoEntriesRequest = await PtoEntriesService.GetByUserAndYear(User.Id, DateTime.Today.Year);
            var thisPtoEntryRequest = await PtoEntriesService.GetyById(Id);

            if (ptoEntriesRequest.Succeeded && thisPtoEntryRequest.Succeeded)
            {
                if (thisPtoEntryRequest.Data.ApplicationUserId != User.Id)
                {
                    Navigation.NavigateTo("/");
                }

                UserPtoEntries = ptoEntriesRequest.Data.ToList();

                PtoDto.PtoEntryId = thisPtoEntryRequest.Data.PtoEntryId;
                PtoDto.ModifiedBy = thisPtoEntryRequest.Data.ModifiedBy;

                PtoDate = thisPtoEntryRequest.Data.PtoDate.ToLocalTime();
                PtoHours = Math.Abs(thisPtoEntryRequest.Data.PtoHours);
                PtoType = thisPtoEntryRequest.Data.PtoType;
            }
            else
            {
                Navigation.NavigateTo("/");
            }
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;

            PtoDto.PtoDate = PtoDate;
            PtoDto.PtoHours = PtoHours * -1;
            PtoDto.PtoType = PtoType;

            var result = await PtoEntriesService.Update(Id, PtoDto);

            if (result.Succeeded)
            {
                Navigation.NavigateTo("/");
            }
            else
            {
                ServerMessages = result.Errors.ToList();
                ShowServerErrors = true;
                DisableSubmit = false;
            }
        }

        private async Task HandleDelete()
        {
            DisableDelete = true;

            var result = await PtoEntriesService.Delete(Id);

            if (result.Succeeded)
            {
                Navigation.NavigateTo("/");
            }
            else
            {
                ServerMessages = result.Errors.ToList();
                ShowServerErrors = true;
                DisableSubmit = false;
            }
        }

        public void GetAvailablePTOHoursByDate(DateTime date)
        {
            VacationHoursAvailable = PtoCalculator.GetVacationHoursAvailableByDate(User.HireDate, User.VacationDaysAccruedPerMonth, UserPtoEntries, date.Date);
            SickHoursAvailable = PtoCalculator.GetSickHoursAvailableByDate(User.HireDate, User.SickHoursAccruedPerMonth, UserPtoEntries, date.Date);
            PersonalHoursAvailable = PtoCalculator.GetPersonalHoursAvailableByDate(User.PersonalDaysPerYear, UserPtoEntries, date.Date);

            UpdateSelectedHoursAvailable(PtoType);
        }

        public void UpdateSelectedHoursAvailable(PtoType ptoType)
        {
            switch (ptoType)
            {
                case PtoType.Vacation:
                    SelectedHoursAvailable = (decimal)VacationHoursAvailable;
                    break;
                case PtoType.Sick:
                    SelectedHoursAvailable = (decimal)SickHoursAvailable;
                    break;
                case PtoType.Personal:
                    SelectedHoursAvailable = (decimal)PersonalHoursAvailable;
                    break;
                default:
                    SelectedHoursAvailable = 8;
                    break;
            }
        }
    }
}
