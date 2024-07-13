using System;
using System.Collections.Generic;
using System.Linq;

namespace ISas.Web.Models
{
    public static class DateHelpers
    {
        //get all dates for a month for the year specified
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
            .Select(day => new DateTime(year, month, day)) // Map each day to a date
            .ToList();
        }

        //get number of week for the selected month by passing in a date value
        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
            while (date.Date.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
                date = date.AddDays(1);
            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        //translate each day to a day number for mapping to week
        public static int GetDateInfo(DateTime now)
        {
            int dayNumber = 0;
            DateTime dt = now.Date;
            string dayStr = Convert.ToString(dt.DayOfWeek);

            if (dayStr.ToLower() == "sunday")
            {
                dayNumber = 0;
            }
            else if (dayStr.ToLower() == "monday")
            {
                dayNumber = 1;
            }
            else if (dayStr.ToLower() == "tuesday")
            {
                dayNumber = 2;
            }
            else if (dayStr.ToLower() == "wednesday")
            {
                dayNumber = 3;
            }
            else if (dayStr.ToLower() == "thursday")
            {
                dayNumber = 4;
            }
            else if (dayStr.ToLower() == "friday")
            {
                dayNumber = 5;
            }
            else if (dayStr.ToLower() == "saturday")
            {
                dayNumber = 6;
            }
            return dayNumber;
        }

        public static string GetMonthByName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "Jan";
                    break;

                case 2:
                    monthName = "Feb";
                    break;

                case 3:
                    monthName = "Mar";
                    break;

                case 4:
                    monthName = "Apr";
                    break;

                case 5:
                    monthName = "May";
                    break;

                case 6:
                    monthName = "Jun";
                    break;

                case 7:
                    monthName = "Jul";
                    break;

                case 8:
                    monthName = "Aug";
                    break;

                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;

                case 11:
                    monthName = "Nov";
                    break;
                case 12:
                    monthName = "Dec";
                    break;
            }

            return monthName;
        }



        public static string getDataDiff(DateTime fromDate, DateTime toDate) //RETURN AS YY-MM-DD
        {

            

            //int years = new DateTime(toDate.Subtract(fromDate).Ticks).Year - 1;
            int years = new DateTime(toDate.Subtract(fromDate.AddDays(-1)).Ticks).Year-1 ;
            DateTime pastYearDate = fromDate.AddYears(years);
            int months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (pastYearDate.AddMonths(i).AddDays(-1) == toDate)
                {
                    months = i;
                    break;
                }
                else if (pastYearDate.AddMonths(i).AddDays(-1) >= toDate)
                {
                    months = i - 1;
                    break;
                }
            }
            int days = toDate.Subtract(pastYearDate.AddMonths(months)).Days+1;
            return string.Format("{0}(Y)-{1}(M)-{2}(D)", years.ToString("00"), months.ToString("00"), days.ToString("00"));
        }

        public static string getDataDiff_days(DateTime fromDate, DateTime toDate) //RETURN AS YY-MM-DD
        {
            int years = new DateTime(toDate.Subtract(fromDate.AddDays(-1)).Ticks).Year-1;
            DateTime pastYearDate = fromDate.AddYears(years);
            int months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (pastYearDate.AddMonths(i).AddDays(-1) == toDate)
                {
                    months = i;
                    break;
                }
                else if (pastYearDate.AddMonths(i).AddDays(-1) >= toDate)
                {
                    months = i - 1;
                    break;
                }
            }
            int days = toDate.Subtract(pastYearDate.AddMonths(months)).Days + 1;
            return days.ToString()+"(D)";
        }
    }
}