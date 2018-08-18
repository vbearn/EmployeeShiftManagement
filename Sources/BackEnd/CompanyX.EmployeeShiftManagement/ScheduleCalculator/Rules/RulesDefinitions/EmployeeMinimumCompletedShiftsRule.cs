using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class EmployeeMinimumCompletedShiftsRule : ISchiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
        {
            if (currentPlanningShift == null)
                throw new ArgumentNullException("currentPlanningShift");

            // if current employee has done two shifts or more, and there exists another employee 
            // with less than two completed shifts, then we should give that employee a chance.
            // so we disqualify current employee from taking another shift until all employees have performed at least 2 shifts.

            var employeeCompletedShifts = pastScheduleItemList.Count(x => x.EmployeeId == currentPlanningShift.EmployeeId);
            if (employeeCompletedShifts >= 2)


            {
                var otherEmployeesCompletedShifts = pastScheduleItemList.GroupBy(x => x.EmployeeId);

                // if there exsits another employee with less than 2 completed shift
                if (otherEmployeesCompletedShifts.Any(x => x.Count() < 2))
                {
                    // current employee is not eligible to take another shift until others perform their required shifts
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }


        }

    }

}
