using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Entities;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Application.PtoEntries.Dtos
{
    public class UpdatePtoEntryDto
    {
        public int PtoEntryId { get; set; }
        public decimal PtoHours { get; set; }
        public PtoType PtoType { get; set; }
        public DateTime PtoDate { get; set; }
        public string? ModifiedBy { get; set; }

        public UpdatePtoEntryDto(PtoEntry entry, string applicationUserId)
        {
            PtoHours = entry.PtoHours;
            PtoType = entry.PtoType;
            PtoDate = entry.PtoDate;
            ModifiedBy = applicationUserId;
        }
    }
}
