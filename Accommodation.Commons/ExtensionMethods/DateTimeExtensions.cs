using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Commons.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static bool IsWorkingDay(this DateTime date)
        {
            try
            {
                return date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static int GetBusinessDays(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            try
            {
                int businessDays = 0;
                DateTime currentDay = firstDay.Date;
                lastDay = lastDay.Date;
                if (firstDay > lastDay)
                    throw new ArgumentException("Last day can't be greater than first day");
                while (currentDay <= lastDay)
                {
                    currentDay = currentDay.AddDays(1);
                    if (currentDay.IsWorkingDay())
                    {
                        //TODO: Also check bankHolidays when we have it
                        businessDays++;
                    }
                }
                return businessDays;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static int GetBusinessDays(this DateTime? firstDay, DateTime? lastDay, params DateTime[] bankHolidays)
        {
            try
            {
                if (firstDay.HasValue && lastDay.HasValue)
                {
                    return GetBusinessDays(firstDay.Value, lastDay.Value, bankHolidays);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static DateTime ConvertTicksToDate(long ticks)
        {
            try
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ticks).ToLocalTime().Date;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }

        }

        public static bool IsBusinessDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static int BusinessDaysTo(this DateTime fromDate, DateTime toDate)
        {
            int ret = 0;
            
            DateTime dt = fromDate;
            
            while (dt <= toDate)
            {
                if (dt.IsBusinessDay()) 
                { 
                    ret++; 
                } 

                dt = dt.AddDays(1);
            }

            return ret;
        }

    }
}
