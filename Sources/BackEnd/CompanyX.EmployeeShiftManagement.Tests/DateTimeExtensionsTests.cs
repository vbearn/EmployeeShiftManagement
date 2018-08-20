using System;
using Xunit;
using CompanyX.EmployeeShiftManagement.Helpers;

namespace CompanyX.EmployeeShiftManagement.Helpers.Tests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData("2018/8/20")]
        [InlineData("2018/8/21")]
        [InlineData("2018/8/25")]
        public void StaticDateNextMondayTest(string dateString)
        {

            var date = DateTime.Parse(dateString);

            var result = date.NextOccuringDayOfWeek(DayOfWeek.Monday);

            Assert.Equal(DateTime.Parse("2018/8/27"), result);

        }
    }
}
