using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Application.Common.Interfaces;
using TimeKeepr.Application.Holidays.Dtos;
using TimeKeepr.Domain.Entities;

namespace TimeKeepr.Application.Holidays
{
    public class HolidaysService
    {
        private readonly IApplicationDbContext _context;

        public HolidaysService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HolidayDto>> GetHolidaysByYearAsync(int year)
        {
            return await _context.Holidays
                .Where(h => h.Year == year)
                .Select(h => new HolidayDto
                {
                    HolidayId = h.HolidayId,
                    Name = h.Name,
                    Date = h.Date,
                    Year = h.Year,
                    Created = h.Created,
                    CreatedBy = h.CreatedBy,
                    LastModified = h.LastModified,
                    ModifiedBy = h.ModifiedBy
                })
                .ToListAsync();
        }

        public async Task<HolidayDto?> GetHolidayByIdAsync(int id)
        {
            return await _context.Holidays
                .Where(p => p.HolidayId == id)
                .Select(h => new HolidayDto
                {
                    HolidayId = h.HolidayId,
                    Name = h.Name,
                    Date = h.Date,
                    Year = h.Year,
                    Created = h.Created,
                    CreatedBy = h.CreatedBy,
                    LastModified = h.LastModified,
                    ModifiedBy = h.ModifiedBy
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateHolidayAsync(CreateHolidayDto holidayDto)
        {
            var newHoliday = new Holiday
            {
                Name = holidayDto.Name,
                Date = holidayDto.Date.ToUniversalTime(),
                Year = holidayDto.Year,
                Created = DateTime.UtcNow,
                CreatedBy = holidayDto.CreatedBy
            };

            await _context.Holidays.AddAsync(newHoliday);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return newHoliday.HolidayId;
            }

            return result;
        }

        public async Task<int> UpdateHolidayAsync(UpdateHolidayDto holidayDto)
        {
            var thisHoliday = await _context.Holidays.FindAsync(holidayDto.HolidayId);

            if (thisHoliday is not null)
            {
                thisHoliday.Name = holidayDto.Name;
                thisHoliday.Date = holidayDto.Date.ToUniversalTime();
                thisHoliday.Year = holidayDto.Year;
                thisHoliday.LastModified = DateTime.UtcNow;
                thisHoliday.ModifiedBy = holidayDto.ModifiedBy;

                _context.Entry(thisHoliday).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    //holiday was found and updated successfully, return holiday id
                    return thisHoliday.HolidayId;
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

        public async Task<int> DeleteHolidayAsync(int id)
        {
            var thisHoliday = await _context.Holidays.FindAsync(id);

            if (thisHoliday is not null)
            {
                _context.Holidays.Remove(thisHoliday);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }
    }
}
