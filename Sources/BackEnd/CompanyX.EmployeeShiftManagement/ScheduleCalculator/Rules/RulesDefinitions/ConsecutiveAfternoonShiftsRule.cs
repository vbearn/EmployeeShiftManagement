using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class ConsecutiveAfternoonShiftsRule : ISchiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
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

}
