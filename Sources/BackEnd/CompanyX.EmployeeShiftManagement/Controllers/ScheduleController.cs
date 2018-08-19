using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules;
using Microsoft.AspNetCore.Mvc;

namespace CompanyX.EmployeeShiftManagement.Controllers
{

    [Route("api/[controller]")]
    internal class ScheduleController : Controller
    {

        #region Fields

        private readonly IShiftCalculator scheduleCalculator;

        #endregion

        #region init

        public ScheduleController(IShiftCalculator scheduleCalculator)
        {
            this.scheduleCalculator = scheduleCalculator;
        }

        #endregion

        #region Contoller Public API

        [HttpPost]
        public JsonResult Post([FromBody]ScheduleCalculateModel scheduleCalculateModel)
        {
            // checking arguments
            //if (scheduleCalculateModel.TotalDays < 1)
            //{
            //  return  StatusCode(400, "TotalDays should be positive");
            //}

            
            // generating a list of employee IDs from 1 to TotalEmployees, and randomizing the numbers so no employee is given a priority in scheduling
            var employeesList = Enumerable.Range(1, scheduleCalculateModel.TotalEmployees).ToArray();
            employeesList = employeesList.RandomizeOrder();

            this.scheduleCalculator.SetEmployeeIdList(employeesList);
            this.scheduleCalculator.SetRules(new List<IShiftRuleBase>() {
                new OneShiftPerDayRule(),
                new ConsecutiveAfternoonShiftsRule(),
                new ConsecutiveDayShiftsEligibleForExemptionRule(),
                new EmployeeMinimumCompletedShiftsRule()
            });

            // caluclating the shifts based on REST params
            var calculatedShifts = this.scheduleCalculator.CalculateShiftsForEmployees(scheduleCalculateModel.TotalDays,
                scheduleCalculateModel.FirstShiftEmployee,
                scheduleCalculateModel.SecondShiftEmployee);

            return MakeResultJsonFromCalculatedShifts(calculatedShifts);

        }

        #endregion

        #region Helper

        private JsonResult MakeResultJsonFromCalculatedShifts(List<EmployeeShiftItem> calculatedShifts)
        {
            var shiftsGroupedByDay = calculatedShifts.GroupBy(x => x.DayNumber).ToList();

            var result = new ScheduleCalculateResultModel()
            {
                Days = new List<ScheduleCalculateResultDayModel>()
            };

            var resultDays = shiftsGroupedByDay.Select(shiftInDay =>
            {

                var firstShiftEmp = shiftInDay.Where(x => x.ShiftNumber == 1).First().EmployeeId;
                var secondShiftEmp = shiftInDay.Where(x => x.ShiftNumber == 2).First().EmployeeId;

                return new ScheduleCalculateResultDayModel()
                {
                    Day = $"Day {(shiftInDay.Key + 1)}",
                    // if this shift matches no employee, it is considered as a "Off Shift" or holiday
                    FirstShiftEmployee = firstShiftEmp.HasValue ? $"Employee {firstShiftEmp}" : "-- Holiday --",
                    SecondShiftEmployee = secondShiftEmp.HasValue ? $"Employee {secondShiftEmp}" : "-- Holiday --",
                };
            });
            result.Days.AddRange(resultDays);

            return Json(result);
        }

        #endregion

    }

}
