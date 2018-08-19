using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime NextOccuringDayOfWeek(this DateTime dt, DayOfWeek nextDayOfWeek)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
                dt = dt.AddDays(1);

            while (dt.DayOfWeek != nextDayOfWeek) dt = dt.AddDays(1);
            return dt;
        }
    }
}
