using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class WeekendsOffRule : IShiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
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

}
