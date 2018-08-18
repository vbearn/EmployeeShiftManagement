using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class ConsecutiveDayShiftsEligibleForExemptionRule : ISchiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
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

}
