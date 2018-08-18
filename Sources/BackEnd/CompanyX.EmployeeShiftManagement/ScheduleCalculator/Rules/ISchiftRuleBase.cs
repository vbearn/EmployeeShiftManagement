using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal interface ISchiftRuleBase
    {
        bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList);
    }

}
