using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TimeKeepr.Application.Common.Logic;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Entities;
using TimeKeepr.Domain.Enums;
using TimeKeepr.Web.Services;

namespace TimeKeepr.Web.Pages.PtoEntry
{
    [Authorize]
    public partial class CreatePtoEntry
    {
        [Inject]
        private IdentityClientService IdentityService { get; set; }
        [Inject]
        private PtoEntriesClientService PtoEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private ApplicationUserDto? User { get; set; }
        private CreatePtoEntryDto PtoDto { get; set; } = new();
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

        protected override async Task OnInitializedAsync()
        {
            User = await IdentityService.GetUserDetails();

            var ptoEntriesRequest = await PtoEntriesService.GetByUserAndYear(User.Id, DateTime.Today.Year);

            if (ptoEntriesRequest.Succeeded)
            {
                UserPtoEntries = ptoEntriesRequest.Data.ToList();

                PtoDate = DateTime.Today;
                PtoHours = 1;
                PtoType = PtoType.Vacation;

                PtoDto.CreatedBy = User.Id;
            }
            else
            {
                Navigation.NavigateTo("/");
            }
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;

            PtoDto.PtoHours = PtoHours * -1;
            PtoDto.PtoType = PtoType;
            PtoDto.PtoDate = PtoDate;

            var result = await PtoEntriesService.Create(PtoDto);

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
