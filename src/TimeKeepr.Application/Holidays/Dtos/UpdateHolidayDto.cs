using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Entities;

namespace TimeKeepr.Application.Holidays.Dtos
{
    public class UpdateHolidayDto
    {
        public int HolidayId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public string? ModifiedBy { get; set; }

        public UpdateHolidayDto(Holiday holiday, string applicationUserId)
        {
            HolidayId = holiday.HolidayId;
            Name = holiday.Name ?? string.Empty;
            Date = holiday.Date;
            Year = holiday.Year;
            ModifiedBy = applicationUserId;
        }
    }
}
