using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Application.Identity.Dtos
{
    public class UpdateApplicationUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal VacationDaysAccruedPerMonth { get; set; }
        public decimal SickHoursAccruedPerMonth { get; set; }
        public int PersonalDaysPerYear { get; set; }
    }
}
