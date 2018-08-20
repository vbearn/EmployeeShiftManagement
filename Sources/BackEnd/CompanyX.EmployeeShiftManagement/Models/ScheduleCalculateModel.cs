using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.EmployeeShiftManagement.ScheduleCalculator;
using Microsoft.AspNetCore.Mvc;

namespace CompanyX.EmployeeShiftManagement.Controllers
{

    public class ScheduleCalculateModel
    {
        public int TotalDays { get; set; }
        public int TotalEmployees { get; set; }
        public int FirstShiftEmployee { get; set; }
        public int SecondShiftEmployee { get; set; }
        public bool HolidaysOff { get; set; }
        public bool RandomizeEmployees { get; set; }

        
    }

}
