using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Domain.Entities
{
    public class PtoEntry
    {
        public int PtoEntryId { get; set; }
        public decimal PtoHours { get; set; }
        public PtoType PtoType { get; set; }
        public DateTime PtoDate { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
