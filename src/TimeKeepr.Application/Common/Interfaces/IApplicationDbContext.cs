using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Domain.Entities;

namespace TimeKeepr.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<PtoEntry> PtoEntries { get; }
        public DbSet<Holiday> Holidays { get; }

        Task<int> SaveChangesAsync();

        EntityEntry Entry(object entity);
    }
}
