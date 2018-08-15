using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CompanyX.EmployeeShiftManagement.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {

        // POST api/values
        [HttpPost]
        public ScheduleCalculateResultModel Post([FromBody]ScheduleCalculateModel scheduleCalculateModel)
        {
            var employeesList = Enumerable.Range(1, scheduleCalculateModel.TotalEmployees).ToArray();
            employeesList = employeesList.RandomizeOrder();

            var sc = new CompanyX.EmployeeShiftManagement.ScheduleCalculator.ScheduleCalculator(employeesList);

            var plannedShifts = sc.CalculateShiftsForEmployees(14,
                scheduleCalculateModel.FirstShiftEmployee,
                scheduleCalculateModel.SecondShiftEmployee);


            var result = new ScheduleCalculateResultModel()
            {
                Days = new List<ScheduleCalculateResultDayModel>()
            };

            var shiftsGroupedByDay = plannedShifts.GroupBy(x => x.DayNumber).ToList();

            var resultDays = shiftsGroupedByDay.Select(shiftInDay =>
            {

                var firstShiftEmp = shiftInDay.Where(x => x.ShiftNumber == 1).First().EmployeeId;
                var secondShiftEmp = shiftInDay.Where(x => x.ShiftNumber == 2).First().EmployeeId;

                return new ScheduleCalculateResultDayModel()
                {
                    Day = $"Day {(shiftInDay.Key + 1)}",
                    FirstShiftEmployee = firstShiftEmp.HasValue ? $"Employee {firstShiftEmp}" : "-- Holiday --",
                    SecondShiftEmployee = secondShiftEmp.HasValue ? $"Employee {secondShiftEmp}" : "-- Holiday --",
                };
            });
            result.Days.AddRange(resultDays);

            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class ScheduleCalculateModel
    {
        public int TotalEmployees { get; set; }
        public int FirstShiftEmployee { get; set; }
        public int SecondShiftEmployee { get; set; }
    }
    public class ScheduleCalculateResultModel
    {
        public List<ScheduleCalculateResultDayModel> Days { get; set; }

    }

    public class ScheduleCalculateResultDayModel
    {
        public string Day { get; set; }
        public string FirstShiftEmployee { get; set; }
        public string SecondShiftEmployee { get; set; }
    }
}
