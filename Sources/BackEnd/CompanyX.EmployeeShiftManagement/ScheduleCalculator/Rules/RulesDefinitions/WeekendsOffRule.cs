using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class WeekendsOffRule : ISchiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
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

}
