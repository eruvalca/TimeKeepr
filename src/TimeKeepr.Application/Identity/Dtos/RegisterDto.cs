using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Application.Identity.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal VacationDaysAccruedPerMonth { get; set; } = 1.25M;
        public decimal SickHoursAccruedPerMonth { get; set; } = 5.33M;
        public int PersonalDaysPerYear { get; set; } = 3;
    }
}
