using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PIC
{
    public static class DateExtensions
    {
        #region 日期类型处理

        public static TimeSpan DateDiffTo(this DateTime from, DateTime to)
        {
            TimeSpan tsFrom = new TimeSpan(from.Ticks);
            TimeSpan tsTo = new TimeSpan(to.Ticks);

            TimeSpan ts = tsTo.Subtract(tsFrom);

            return ts;
        }

        /// <summary>
        /// 获取年中的周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(this DateTime source)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                source, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        /// <summary>
        /// 获取年中的季度
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetQuarterOfYear(this DateTime source)
        {
            int month = source.Month;
            if (month >= 0 && month <= 3)
            {
                return 1;
            }
            else if (month >= 4 && month <= 6)
            {
                return 2;
            }
            else if (month >= 7 && month <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        #endregion
    }
}
