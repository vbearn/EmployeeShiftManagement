using CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator
{
    internal class ScheduleCalculator
    {
        int currentDayNumber = 0;
        int currentShift = 1;

        ScheduleRuleManager ruleManager;
        Queue<int> emplyeeIdSeedQueue;
        List<ScheduleShiftItem> plannedScheduleShifts;


        public ScheduleCalculator(IEnumerable<int> employeeIds)
        {
            this.ruleManager = new ScheduleRuleManager();
            this.emplyeeIdSeedQueue = new Queue<int>(employeeIds);
            this.plannedScheduleShifts = new List<ScheduleShiftItem>();
        }

        public List<ScheduleShiftItem> CalculateShiftsForEmployees(int numberOfDays, int firstShiftEmployee, int secondShiftEmployee)
        {
            plannedScheduleShifts.Add(new ScheduleShiftItem()
            {
                DayNumber = 0,
                ShiftNumber = 1,
                EmployeeId = firstShiftEmployee,
            });

            plannedScheduleShifts.Add(new ScheduleShiftItem()
            {
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

                var currentPlanningScheduleShift = new ScheduleShiftItem()
                {
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

    }
}
