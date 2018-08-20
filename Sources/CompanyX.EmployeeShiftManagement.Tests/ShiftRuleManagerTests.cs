using System;
using Xunit;
using CompanyX.EmployeeShiftManagement.Helpers;
using System.Collections.Generic;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules.Tests
{
    public class ShiftRuleManagerTests
    {

        [Fact]
        public void SatisfyEverythingRuleTest()
        {

            var shiftRuleManager = new ShiftRuleManager();
            shiftRuleManager.SetRules(new List<IShiftRuleBase>() { new SatisfyEverythingRule() });
            var testShift = new EmployeeShiftItem();

            var result = shiftRuleManager.IsEmployeeEligibleForShift(testShift, null);

            Assert.True(result);
        }

        [Fact]
        public void OneSatisfyNothingRuleInMultipleSatisfyEverythingRuleTest()
        {

            var shiftRuleManager = new ShiftRuleManager();
            shiftRuleManager.SetRules(new List<IShiftRuleBase>() {
                new SatisfyEverythingRule(),
                new SatisfyEverythingRule(),
                new SatisfyNothingRule(),
                new SatisfyEverythingRule(),
                new SatisfyEverythingRule(),

            });
            var testShift = new EmployeeShiftItem();

            var result = shiftRuleManager.IsEmployeeEligibleForShift(testShift, null);

            Assert.False(result);
        }

        [Fact]
        public void SatisfyNothingRuleTest()
        {

            var shiftRuleManager = new ShiftRuleManager();
            shiftRuleManager.SetRules(new List<IShiftRuleBase>() { new SatisfyNothingRule() });
            var testShift = new EmployeeShiftItem();

            var result = shiftRuleManager.IsEmployeeEligibleForShift(testShift, null);

            Assert.False(result);
        }

    }

    internal class SatisfyEverythingRule : IShiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
        {
            return true;
        }
    }

    internal class SatisfyNothingRule : IShiftRuleBase
    {
        public bool SatisfiesRule(EmployeeShiftItem currentPlanningShift, IReadOnlyList<EmployeeShiftItem> pastScheduleItemList)
        {
            return false;
        }
    }

}
