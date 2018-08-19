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
            var dayInWeek = currentPlanningShift.Date.DayOfWeek;

            if (dayInWeek ==  DayOfWeek.Saturday || dayInWeek ==  DayOfWeek.Sunday)
                return false;
            else
                return true;

        }

    }

}
