using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Domain.Entities;
using TimeKeepr.Domain.Enums;

namespace TimeKeepr.Application.Common.Logic
{
    public static class PtoCalculator
    {
        private const decimal MaxYearlySickHours = 56M;

        public static decimal GetMaxAccruedHoursAllowedByType(PtoType ptoType, decimal vacationDaysAccruedPerMonth = 1.25M)
        {
            // TODO: set parameter to default vacationDaysAccruedPerMonth, optional param
            return ptoType switch
            {
                PtoType.Vacation => (12 * vacationDaysAccruedPerMonth) * 8,
                PtoType.Sick => MaxYearlySickHours,
                PtoType.Personal => 24M,
                PtoType.VacationCarryOver => 40M,
                _ => 0M,
            };
        }

        public static decimal GetVacationHoursCarriedOverByYear(IEnumerable<PtoEntry> ptoEntries, int year)
        {
            return ptoEntries
                .Where(p => p.PtoType == PtoType.VacationCarryOver
                    && p.PtoDate.Year == year)
                .Sum(p => p.PtoHours);
        }

        public static decimal GetVacationHoursUsedInFirstQuarterByYear(IEnumerable<PtoEntry> ptoEntries, int year)
        {
            return ptoEntries
                .Where(p => p.PtoType == PtoType.Vacation
                    && p.PtoDate.Year == year
                    && p.PtoDate.Date <= new DateTime(year, 3, 31).Date)
                .Sum(p => p.PtoHours);
        }
        public static decimal GetHoursPlannedAfterDateByType(IEnumerable<PtoEntry> ptoEntries, DateTime afterDate, PtoType ptoType)
        {
            afterDate = afterDate.ToUniversalTime();

            return ptoEntries
                .Where(p => p.PtoType == ptoType
                    && p.PtoDate.Year == afterDate.Year
                    && p.PtoDate.Date > afterDate.Date)
                .Sum(p => p.PtoHours);
        }

        public static decimal GetVacationHoursAvailableByDate(DateTime hireDate, decimal vacationDaysAccruedPerMonth,
            IEnumerable<PtoEntryDto> ptoEntries, DateTime asOfDate)
        {
            hireDate = hireDate.ToUniversalTime();
            asOfDate = asOfDate.ToUniversalTime();

            var vacationCarryOverExpirationDate = new DateTime(asOfDate.Year, 3, 31);
            decimal vacationHoursAccrued;

            //if hire date is after jan of year given, calc vacation hours starting from month hired, else calc from month given
            if (hireDate.Date > new DateTime(asOfDate.Year, 1, 1).Date && hireDate.Month > 1)
            {
                vacationHoursAccrued = (asOfDate.Date.Month - hireDate.Month + 1) * vacationDaysAccruedPerMonth * 8;
            }
            else
            {
                vacationHoursAccrued = (asOfDate.Date.Month * vacationDaysAccruedPerMonth) * 8;
            }

            var vacationHoursCarriedOver = ptoEntries
                .Where(p => p.PtoType == PtoType.VacationCarryOver
                    && p.PtoDate.Year == asOfDate.Year)
                .Sum(p => p.PtoHours);

            var vacationHoursUsedAgainstCarriedOver = ptoEntries
                .Where(p => p.PtoType == PtoType.Vacation
                    && p.PtoDate.Year == asOfDate.Year
                    && p.PtoDate.Date <= vacationCarryOverExpirationDate.Date
                    && p.PtoDate.Date <= asOfDate.Date)
                .Sum(p => p.PtoHours);

            var vacationHoursUsedAfterFirstQuarter = ptoEntries
                .Where(p => p.PtoType == PtoType.Vacation
                    && p.PtoDate.Year == asOfDate.Year
                    && p.PtoDate.Date > vacationCarryOverExpirationDate.Date
                    && p.PtoDate.Date <= asOfDate.Date)
                .Sum(p => p.PtoHours);

            var carryOverBalance = vacationHoursCarriedOver + vacationHoursUsedAgainstCarriedOver;
            decimal vacationHoursAvailable = vacationHoursAccrued + vacationHoursUsedAfterFirstQuarter;

            //if user carries a negative carry over balance (used all carry over vaction time)
            //or currently in first quarter of year, add carry over balance to hours available
            if (carryOverBalance < 0 || asOfDate.Date <= vacationCarryOverExpirationDate.Date)
            {
                vacationHoursAvailable += carryOverBalance;
            }

            return vacationHoursAvailable;
        }

        public static decimal GetSickHoursAvailableByDate(DateTime hireDate, decimal sickHoursAccruedPerMonth,
            IEnumerable<PtoEntry> ptoEntries, DateTime asOfDate)
        {
            hireDate = hireDate.ToUniversalTime();
            asOfDate = asOfDate.ToUniversalTime();

            decimal sickHoursAccrued;

            //if hire date is after jan of year given, calc sick hours starting from month hired, else calc from month given
            if (hireDate.Date > new DateTime(asOfDate.Year, 1, 1).Date && hireDate.Month > 1)
            {
                sickHoursAccrued = (asOfDate.Date.Month - hireDate.Month + 1) * sickHoursAccruedPerMonth;
            }
            else
            {
                sickHoursAccrued = asOfDate.Date.Month * sickHoursAccruedPerMonth;
            }

            //max sick hours accrued is 56 hours (7 days)
            if (sickHoursAccrued > GetMaxAccruedHoursAllowedByType(PtoType.Sick))
            {
                sickHoursAccrued = GetMaxAccruedHoursAllowedByType(PtoType.Sick);
            }

            var sickHoursUsed = ptoEntries
                .Where(p => p.PtoType == PtoType.Sick
                    && p.PtoDate.Year == asOfDate.Year
                    && p.PtoDate.Date <= asOfDate.Date)
                .Sum(p => p.PtoHours);

            decimal sickHoursAvailable = sickHoursAccrued + sickHoursUsed;

            return sickHoursAvailable;
        }

        public static decimal GetPersonalHoursAvailableByDate(int personalDaysPerYear, IEnumerable<PtoEntry> ptoEntries,
            DateTime asOfDate)
        {
            asOfDate = asOfDate.ToUniversalTime();

            var personalHoursused = ptoEntries
                .Where(p => p.PtoType == PtoType.Personal
                    && p.PtoDate.Year == asOfDate.Year
                    && p.PtoDate.Date <= asOfDate.Date)
                .Sum(p => p.PtoHours);

            decimal personalHoursAvailable = (personalDaysPerYear * 8) + personalHoursused;

            return personalHoursAvailable;
        }

        public static decimal GetRemainingVacationHoursCarriedOverByYear(IEnumerable<PtoEntry> ptoEntries, int year)
        {
            var vacationCarryOverExpirationDate = new DateTime(year, 3, 31);

            var vacationHoursCarriedOver = ptoEntries
                   .Where(p => p.PtoType == PtoType.VacationCarryOver
                       && p.PtoDate.Year == year)
                   .Sum(p => p.PtoHours);

            var vacationHoursUsedAgainstCarriedOver = ptoEntries
                .Where(p => p.PtoType == PtoType.Vacation
                    && p.PtoDate.Year == year
                    && p.PtoDate.Date <= vacationCarryOverExpirationDate.Date)
                .Sum(p => p.PtoHours);

            return vacationHoursCarriedOver + vacationHoursUsedAgainstCarriedOver;
        }
    }
}
