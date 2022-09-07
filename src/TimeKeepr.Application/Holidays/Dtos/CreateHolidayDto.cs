using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Application.Holidays.Dtos
{
    public class CreateHolidayDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public string CreatedBy { get; set; }

        public CreateHolidayDto(string applicationUserId)
        {
            CreatedBy = applicationUserId;
        }
    }
}
