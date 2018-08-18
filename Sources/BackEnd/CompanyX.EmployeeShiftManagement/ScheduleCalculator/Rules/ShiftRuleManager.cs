using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules
{

    internal class ShiftRuleManager 
    {

        #region Fields

        private List<ISchiftRuleBase> rules;
        public void SetRules(List<ISchiftRuleBase> rules)
        {
            this.rules = rules;
        }

        #endregion

        #region init

        public ShiftRuleManager()
        {
           
        }

        #endregion

        #region Reule Checking

        public bool IsEmployeeEligibleForShift(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
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

        #endregion

    }

}
