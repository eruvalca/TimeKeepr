using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Application.Common.Interfaces;
using TimeKeepr.Application.Common.Logic;
using TimeKeepr.Application.Holidays.Dtos;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Entities;

namespace TimeKeepr.Application.PtoEntries
{
    public class PtoEntriesService
    {
        private readonly IApplicationDbContext _context;

        public PtoEntriesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PtoEntryDto>> GetUserPtoEntriesByYearAsync(string applicationUserId, int year)
        {
            return await _context.PtoEntries
                .Where(p => p.ApplicationUserId == applicationUserId
                    && p.PtoDate.Year == year)
                .Select(p => new PtoEntryDto
                {
                    PtoEntryId = p.PtoEntryId,
                    PtoHours = p.PtoHours,
                    PtoType = p.PtoType,
                    PtoDate = p.PtoDate,
                    Created = p.Created,
                    CreatedBy = p.CreatedBy,
                    LastModified = p.LastModified,
                    ModifiedBy = p.ModifiedBy,
                    ApplicationUserId = p.ApplicationUserId
                })
                .ToListAsync();
        }

        public async Task<PtoEntryDto?> GetPtoEntryByIdAsync(int id)
        {
            return await _context.PtoEntries
                .Where(p => p.PtoEntryId == id)
                .Select(p => new PtoEntryDto
                {
                    PtoEntryId = p.PtoEntryId,
                    PtoHours = p.PtoHours,
                    PtoType = p.PtoType,
                    PtoDate = p.PtoDate,
                    Created = p.Created,
                    CreatedBy = p.CreatedBy,
                    LastModified = p.LastModified,
                    ModifiedBy = p.ModifiedBy,
                    ApplicationUserId = p.ApplicationUserId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreatePtoEntryAsync(CreatePtoEntryDto ptoEntryDto)
        {
            var newPtoEntry = new PtoEntry
            {
                PtoHours = ptoEntryDto.PtoHours,
                PtoType = ptoEntryDto.PtoType,
                PtoDate = ptoEntryDto.PtoDate.Date.ToUniversalTime(),
                Created = DateTime.UtcNow,
                CreatedBy = ptoEntryDto.CreatedBy,
                ApplicationUserId = ptoEntryDto.CreatedBy
            };

            await _context.PtoEntries.AddAsync(newPtoEntry);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return newPtoEntry.PtoEntryId;
            }

            return result;
        }

        public async Task<int> UpdatePtoEntryAsync(UpdatePtoEntryDto ptoEntryDto)
        {
            var thisPtoEntry = await _context.PtoEntries.FindAsync(ptoEntryDto.PtoEntryId);

            if (thisPtoEntry is not null)
            {
                thisPtoEntry.PtoHours = ptoEntryDto.PtoHours;
                thisPtoEntry.PtoType = ptoEntryDto.PtoType;
                thisPtoEntry.PtoDate = ptoEntryDto.PtoDate.Date.ToUniversalTime();
                thisPtoEntry.LastModified = DateTime.UtcNow;
                thisPtoEntry.ModifiedBy = ptoEntryDto.ModifiedBy;

                _context.Entry(thisPtoEntry).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    //entry was found and updated successfully, return entry id
                    return thisPtoEntry.PtoEntryId;
                }

                //entry was found but not updated, return 0
                return result;
            }
            else
            {
                //entry was not found, return -1
                return -1;
            }
        }

        public async Task<int> DeletePtoEntryAsync(int id)
        {
            var thisPtoEntry = await _context.PtoEntries.FindAsync(id);

            if (thisPtoEntry is not null)
            {
                _context.PtoEntries.Remove(thisPtoEntry);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<bool> AddYearlyVacationCarryOverAsync(IEnumerable<ApplicationUserDto> users, DateTime asOfDate)
        {
            var newPtoEntries = new List<PtoEntry>();

            foreach (var appUser in users)
            {
                var previousYearPtoEntries = await GetUserPtoEntriesByYearAsync(appUser.Id, asOfDate.Year - 1);

                var vacationToCarryOver = PtoCalculator.GetVacationHoursAvailableByDate(appUser.HireDate,
                    appUser.VacationDaysAccruedPerMonth, previousYearPtoEntries, asOfDate.AddDays(-1));

                var newPtoEntry = new PtoEntry
                {
                    PtoHours = vacationToCarryOver > 40 ? 40 : vacationToCarryOver,
                    PtoType = Domain.Enums.PtoType.VacationCarryOver,
                    PtoDate = asOfDate,
                    Created = DateTime.UtcNow,
                    CreatedBy = appUser.Id,
                    ApplicationUserId = appUser.Id
                };

                newPtoEntries.Add(newPtoEntry);
            }

            await _context.PtoEntries.AddRangeAsync(newPtoEntries);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
