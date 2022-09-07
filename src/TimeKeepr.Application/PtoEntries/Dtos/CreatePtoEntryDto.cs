using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Application.PtoEntries.Dtos
{
    public class CreatePtoEntryDto
    {
        public decimal PtoHours { get; set; }
        public PtoType PtoType { get; set; }
        public DateTime PtoDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
