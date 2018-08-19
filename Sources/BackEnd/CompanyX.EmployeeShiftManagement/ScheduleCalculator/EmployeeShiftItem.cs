using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.ScheduleCalculator
{


    internal class EmployeeShiftItem
    {
        public DateTime Date { get; set; }
        public int DayNumber { get; set; }

        // The shift number for current day: First Shift is 1, Second Shift is 2
        public int ShiftNumber { get; set; }
        public long? EmployeeId { get; set; }
    }

}
