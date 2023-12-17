using API.StockAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Domain.Utilities
{
    public class DateCalculator : IDateCalculator
    {
        public string GetLastBusinessDay(string type)
        {
            return type switch
            {
                "weekly" => GetLastBusinessDayOfLastWeek(),
                "monthly" => GetLastBusinessDayOfLastMonth(),
                _ => ""
            };
        }
        private string GetLastBusinessDayOfLastWeek()
        {
            var currentDate = DateTime.UtcNow;

            DateTime lastWeekEndDate = currentDate.AddDays(-((int)currentDate.DayOfWeek + 1));

            DateTime lastFriday = lastWeekEndDate.AddDays(-1);

            return lastFriday.ToString("yyyy-MM-dd");
        }
        private string GetLastBusinessDayOfLastMonth()
        {
            var lastDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month - 1, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month - 1));

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                lastDayOfMonth = lastDayOfMonth.AddDays(-2);
            }

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
            {
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);
            }

            return lastDayOfMonth.ToString("yyyy-MM-dd");
        }
    }
}
