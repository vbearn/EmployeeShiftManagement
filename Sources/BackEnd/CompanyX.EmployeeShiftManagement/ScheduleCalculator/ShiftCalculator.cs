using CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


[assembly: InternalsVisibleTo("CompanyX.EmployeeShiftManagement.Tests")]
namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator
{

    internal interface IShiftCalculator
    {

        List<EmployeeShiftItem> CalculateShiftsForEmployees(DateTime startDate, int numberOfDays, int firstShiftEmployee, int secondShiftEmployee);
        void SetEmployeeIdList(IEnumerable<int> employeeIds);
        void SetRules(List<IShiftRuleBase> rules);

    }

    /// <summary>
    /// Calculator for employee shifts on a given schedule
    /// </summary>
    internal class ShiftCalculator : IShiftCalculator
    {

        #region Fields

        int currentDayNumber = 0;
        int currentShift = 1;

        ShiftRuleManager ruleManager;
        public void SetRules(List<IShiftRuleBase> rules)
        {
            this.ruleManager.SetRules(rules);
        }

        Queue<int> emplyeeIdSeedQueue;
        public void SetEmployeeIdList(IEnumerable<int> employeeIds)
        {
            this.emplyeeIdSeedQueue = new Queue<int>(employeeIds);
        }

        List<EmployeeShiftItem> plannedScheduleShifts;

        #endregion

        #region init

        public ShiftCalculator()
        {
            this.ruleManager = new ShiftRuleManager();
            this.plannedScheduleShifts = new List<EmployeeShiftItem>();
        }

        #endregion

        #region Caluclations

        /// <summary>
        /// Schedules the shifts for employees given the parameters
        /// </summary>
        /// <param name="numberOfDays">total number of days to calculate</param>
        /// <param name="firstShiftEmployee">the employee which should be pre-allocated on first shift of day 1</param>
        /// <param name="secondShiftEmployee">the employee which should be pre-allocated on second shift of day 1</param>
        /// <returns></returns>
        public List<EmployeeShiftItem> CalculateShiftsForEmployees(DateTime startDate, int numberOfDays, int firstShiftEmployee, int secondShiftEmployee)
        {
            plannedScheduleShifts.Add(new EmployeeShiftItem()
            {
                Date = startDate,
                DayNumber = 0,
                ShiftNumber = 1,
                EmployeeId = firstShiftEmployee,
            });

            plannedScheduleShifts.Add(new EmployeeShiftItem()
            {
                Date = startDate,
                DayNumber = 0,
                ShiftNumber = 2,
                EmployeeId = secondShiftEmployee,
            });

            this.currentDayNumber = 1;
            this.currentShift = 1;

            int employeesTestedForEligibility = 0;

            while (this.currentDayNumber < numberOfDays)
            {
                var currentPlannigEmployeeId = emplyeeIdSeedQueue.Dequeue();
                emplyeeIdSeedQueue.Enqueue(currentPlannigEmployeeId);

                var currentPlanningScheduleShift = new EmployeeShiftItem()
                {
                    Date = startDate.AddDays(currentDayNumber),
                    DayNumber = currentDayNumber,
                    ShiftNumber = currentShift,
                    EmployeeId = currentPlannigEmployeeId,
                };

                if (ruleManager.IsEmployeeEligibleForShift(currentPlanningScheduleShift, plannedScheduleShifts))
                {
                    plannedScheduleShifts.Add(currentPlanningScheduleShift);

                    employeesTestedForEligibility = 0;
                    GotoNextShift();
                }
                else
                {
                    employeesTestedForEligibility++;

                    // check if we have tested all employees
                    if (employeesTestedForEligibility > emplyeeIdSeedQueue.Count)
                    {
                        // no employee is eligible for this shift. we should go to the next shift
                        currentPlanningScheduleShift.EmployeeId = null;
                        plannedScheduleShifts.Add(currentPlanningScheduleShift);

                        employeesTestedForEligibility = 0;
                        GotoNextShift();
                    }
                }
            }

            return this.plannedScheduleShifts;

        }

        private void GotoNextShift()
        {
            currentShift++;
            if (currentShift > 2)
            {
                currentShift = 1;
                currentDayNumber++;
            }
        }

        #endregion

    }

}
