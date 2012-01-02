// This is some weird example where the parameters were not being properly considered as variables.
// It had to do with the consideration of Location start/end and the NewMethod being constructed not 
// having proper location info.  
using System;

namespace AgentRalph.ExtractMethodTests.TestData
{
    static class ParamsNotConsideredDueToLocationBug
    {
        public static DateTime Target(this int kddkdk, DateTime date, int monthsBefore)
        {
            /* BEGIN */
            var dayInPrevMonth = FindDayInMonth(date.Year, date.Month, kddkdk);
            var prev = date.AddMonths(monthsBefore);
            return new DateTime(prev.Year, prev.Month, dayInPrevMonth);
            /* END */
        }

        public static DateTime Expected(this int kddkdk, DateTime date, int monthsBefore)
        {
            var dayInPrevMonth = FindDayInMonth(date.Year, date.Month, kddkdk);
            var prev = date.AddMonths(monthsBefore);
            return new DateTime(prev.Year, prev.Month, dayInPrevMonth);
        }

        private static int FindDayInMonth(int year, int month, int @this)
        {
            throw new NotImplementedException();
        }
    }
}