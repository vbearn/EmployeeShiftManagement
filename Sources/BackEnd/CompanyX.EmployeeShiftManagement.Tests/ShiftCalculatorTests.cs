using System;
using Xunit;
using CompanyX.EmployeeShiftManagement.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules.Tests
{
    public class ShiftCalculatorTests
    {

        /// <summary>
        /// Tests the result of shift calculator for 5 employees on 7 days starting from monday
        /// </summary>
        [Fact]
        public void SpecificDateCalculateResultTest()
        {

            var shiftCalculator = new ShiftCalculator();
            shiftCalculator.SetRules(new List<IShiftRuleBase>() {
                new OneShiftPerDayRule(),
                new ConsecutiveAfternoonShiftsRule(),
                new ConsecutiveDayShiftsEligibleForExemptionRule(),
                new EmployeeMinimumCompletedShiftsRule(),
                new WeekendsOffRule(),
            });
            var employeesList = Enumerable.Range(1, 5).ToArray();
            shiftCalculator.SetEmployeeIdList(employeesList);

            // Monday
            var dateToStart = new DateTime(2018, 8, 20).Date;

            var result = shiftCalculator.CalculateShiftsForEmployees(dateToStart, 7, 1, 2);

            // total shifts
            Assert.Equal(14, result.Count);

            // first day
            Assert.Equal(1, result[0].EmployeeId);
            Assert.Equal(2, result[1].EmployeeId);

            // third day
            Assert.Equal(4, result[4].EmployeeId);
            Assert.Equal(5, result[5].EmployeeId);

            // Weekends
            Assert.Null(result[10].EmployeeId);
            Assert.Null(result[11].EmployeeId);
            Assert.Null(result[12].EmployeeId);
            Assert.Null(result[13].EmployeeId);

        }

        // TODO: add another tests for checking rules against shift calculator's result
    }

}
