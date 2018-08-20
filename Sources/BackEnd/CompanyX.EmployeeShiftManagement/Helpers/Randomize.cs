using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.EmployeeShiftManagement.Helpers
{
    public static class Randomize
    {
        public static T[] RandomizeOrder<T>(this T[] array)
        {
            var rand = new Random();
            return array.Select(x => new { x, r = rand.Next() })
                                           .OrderBy(x => x.r)
                                           .Select(x => x.x)
                                           .ToArray();
        }
    }
}
