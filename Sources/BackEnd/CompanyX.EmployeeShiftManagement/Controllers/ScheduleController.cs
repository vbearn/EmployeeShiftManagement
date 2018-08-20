using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.EmployeeShiftManagement.Helpers;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator.Rules;
using Microsoft.AspNetCore.Mvc;

namespace CompanyX.EmployeeShiftManagement.Controllers
{

    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {

        #region Fields

        private readonly IServiceProvider injector;

        #endregion

        #region init

        public ScheduleController(IServiceProvider injector)
        {
            this.injector = injector;

        }

        #endregion

        #region Contoller Public API

        [HttpGet]
        public string Get()
        {
            return "Service test OK. To consume the service, use POST endpoint and post the required data as ScheduleCalculateModel json model.";
        }

        [HttpPost]
        public JsonResult Post([FromBody]ScheduleCalculateModel scheduleCalculateModel)
        {
            // checking arguments
            //if (scheduleCalculateModel.TotalDays < 1)
            //{
            //   throw new Exception(400, "TotalDays should be positive");
            //}


            // creating a calculator instance from dependency injector
            var scheduleCalculator = injector.GetService(typeof(IShiftCalculator)) as IShiftCalculator;

            // setting calculation rules
            var rules = new List<IShiftRuleBase>() {
                new OneShiftPerDayRule(),
                new ConsecutiveAfternoonShiftsRule(),
                new ConsecutiveDayShiftsEligibleForExemptionRule(),
                new EmployeeMinimumCompletedShiftsRule()
            };

            if (scheduleCalculateModel.HolidaysOff)
            {
                rules.Add(new WeekendsOffRule());
            }

            scheduleCalculator.SetRules(rules);

            // generating a list of employee IDs from 1 to TotalEmployees, and randomizing the numbers so no employee is given a priority in scheduling
            var employeesList = Enumerable.Range(1, scheduleCalculateModel.TotalEmployees).ToArray();
            if (scheduleCalculateModel.RandomizeEmployees)
            {
                employeesList = employeesList.RandomizeOrder();
            }

            scheduleCalculator.SetEmployeeIdList(employeesList);


            // find the next working day of the upcoming week
            var nextMonday = DateTime.Now.NextOccuringDayOfWeek(DayOfWeek.Monday).Date;

            // caluclating the shifts based on REST params
            var calculatedShifts = scheduleCalculator.CalculateShiftsForEmployees(nextMonday, scheduleCalculateModel.TotalDays,
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
                    Day = $"Day {(shiftInDay.Key + 1)} ({shiftInDay.First().Date.ToShortDateString()})",
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
