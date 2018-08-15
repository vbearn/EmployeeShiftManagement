using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{
    internal class ScheduleRuleManager
    {
        private List<ISchdeuleRuleBase> rules;
        public ScheduleRuleManager()
        {
            rules = new List<ISchdeuleRuleBase>() {
                new OneShiftPerDayRule(),
                new ConsecutiveAfternoonShiftsRule(),
                new ConsecutiveDayShiftsEligibleForExemptionRule()
            };
        }

        public bool IsEmployeeEligibleForShift(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList)
        {
            foreach (var rule in this.rules)
            {
                if (!rule.SatisfiesRule(currentPlanningShift, pastScheduleItemList))
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal interface ISchdeuleRuleBase
    {
        bool SatisfiesRule(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList);
    }

    internal class WeekendsOffRule : ISchdeuleRuleBase
    {
        public bool SatisfiesRule(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList)
        {
            if (currentPlanningShift == null)
                throw new ArgumentNullException("currentPlanningShift");

            var hasOtherShiftsToday = pastScheduleItemList.Any(x =>
            x.EmployeeId == currentPlanningShift.EmployeeId && x.DayNumber == currentPlanningShift.DayNumber);

            if (hasOtherShiftsToday)
            {
                // this emplyee had another shift today and is not eligible for another working shift
                return false;
            }
            else
            {
                return true;
            }
        }

    }

    internal class OneShiftPerDayRule : ISchdeuleRuleBase
    {
        public bool SatisfiesRule(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList)
        {
            if (currentPlanningShift == null)
                throw new ArgumentNullException("currentPlanningShift");

            // Weekends should be excluded from shift planning
            var dayInWeek = currentPlanningShift.DayNumber % 7;

            // We assume day zero is Monday (start of the week), so weekends are 5 and 6 days in the week. 
            if (dayInWeek == 5 || dayInWeek == 6)
                return false;
            else
                return true;

        }

    }

    internal class ConsecutiveAfternoonShiftsRule : ISchdeuleRuleBase
    {
        public bool SatisfiesRule(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList)
        {
            if (currentPlanningShift == null)
                throw new ArgumentNullException("currentPlanningShift");

            if (currentPlanningShift.ShiftNumber != 1 && currentPlanningShift.ShiftNumber != 2)
                throw new ArgumentOutOfRangeException("currentPlanningShift.ShiftNumber");

            if (currentPlanningShift.DayNumber == 0)
            {
                // employee is being considered for the first day of schedule
                return true;
            }
            else if (currentPlanningShift.ShiftNumber == 1)
            {
                // this is not an afternoon shift
                return true;
            }
            else // currentPlanningShift.ShiftNumber is 2
            {
                var hasAfternoonShiftsYesterday = pastScheduleItemList.Any(x =>
                    x.EmployeeId == currentPlanningShift.EmployeeId && x.DayNumber == currentPlanningShift.DayNumber - 1 &&
                    x.ShiftNumber == 2);

                if (hasAfternoonShiftsYesterday)
                {
                    // this emplyee had afternoon shift yesterday and is not eligible for another afternoon working shift today
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

    }

    internal class ConsecutiveDayShiftsEligibleForExemptionRule : ISchdeuleRuleBase
    {
        public bool SatisfiesRule(ScheduleShiftItem currentPlanningShift, IReadOnlyList<ScheduleShiftItem> pastScheduleItemList)
        {
            if (currentPlanningShift == null)
                throw new ArgumentNullException("currentPlanningShift");

            if (currentPlanningShift.ShiftNumber != 1 && currentPlanningShift.ShiftNumber != 2)
                throw new ArgumentOutOfRangeException("currentPlanningShift.ShiftNumber");

            if (currentPlanningShift.DayNumber < 2)
            {
                // employee is being considered for the first or second day of the schedule and is not eligible for exemption
                return true;
            }
            else
            {
                var hasShiftsYesterday = pastScheduleItemList.Any(x =>
                    x.EmployeeId == currentPlanningShift.EmployeeId && x.DayNumber == currentPlanningShift.DayNumber - 1);

                var hasShiftsTwoDaysAgo = pastScheduleItemList.Any(x =>
                    x.EmployeeId == currentPlanningShift.EmployeeId && x.DayNumber == currentPlanningShift.DayNumber - 2);

                if (hasShiftsYesterday && hasShiftsTwoDaysAgo)
                {
                    // this emplyee had two consecutive day shifts and is eligible for exemption today (should no be selected today)
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

    }

    internal class ScheduleShiftItem
    {
        public int DayNumber { get; set; }

        // The shift number for current day: First Shift is 1, Second Shift is 2
        public int ShiftNumber { get; set; }
        public long? EmployeeId { get; set; }
    }

    //internal enum ScheduleShifts
    //{
    //    FirstShift = 1,
    //    SecondShift = 2,
    //}
}
