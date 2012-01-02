using System;
using System.Collections.Generic;
using System.Text;

namespace Ralph.Test.Project
{
    static class Issue5
    {
        public static DateTime DayInMonthAfter(this int @this, DateTime date)
        {
            return DayInMonthsAfter(@this, date, 1);
        }

        // When this method is replaced with a call to DayInMonthsBeforeOrSameDay, it's using the
        // names of parameters as DayInMonthsBeforeOrSameDay defined them.  Should be using the names
        // defined on DayInMonthsAfter.
        public static DateTime DayInMonthsAfter(this int @this, DateTime date, int numberOfMonthsAfter)
        {
            var dayInPrevMonth = FindDayInMonth(date.Year, date.Month, @this);
            var prev = date.AddMonths(numberOfMonthsAfter);
            return new DateTime(prev.Year, prev.Month, dayInPrevMonth);
        }

        public static DateTime DayInMonthBeforeOrSameDay(this int @this, DateTime date)
        {
            return DayInMonthsBeforeOrSameDay(@this, date, -11);
        }

        public static DateTime DayInMonthsBeforeOrSameDay(this int this1, DateTime date, int monthsBefore)
        {
            var dayInPrevMonth = FindDayInMonth(date.Year, date.Month, this1);
            var prev = date.AddMonths(monthsBefore);
            return new DateTime(prev.Year, prev.Month, dayInPrevMonth);
        }

        private static int FindDayInMonth(int year, int month, int @this)
        {
            throw new NotImplementedException();
        }
    }
}
