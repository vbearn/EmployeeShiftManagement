using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator;
using Microsoft.AspNetCore.Mvc;

namespace CompanyX.EmployeeShiftManagement.Controllers
{

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
