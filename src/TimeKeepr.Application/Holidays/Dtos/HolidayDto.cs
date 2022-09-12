using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Application.Holidays.Dtos
{
    public class HolidayDto
    {
        public int HolidayId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
