using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xalendar.Api.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Month _month;

        private IReadOnlyList<Day?>? _days;
        public IReadOnlyList<Day?> Days => _days ??= GetDaysOfContainer();
        public IReadOnlyList<string> DaysOfWeek { get; }

        public DateTime FirstDay => Days.First(day => day is {})!.DateTime;

        public DateTime LastDay => Days.Last(day => day is {})!.DateTime.AddHours(23).AddMinutes(59).AddSeconds(59);

        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);

            var cultureInfo = CultureInfo.CurrentCulture;
            var dateTimeFormat = cultureInfo.DateTimeFormat;
            DaysOfWeek = new List<string>
            {
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Sunday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Monday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Tuesday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Wednesday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Thursday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Friday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Saturday).Substring(0, 3).ToUpper()
            };
        }

        private IReadOnlyList<Day?> GetDaysOfContainer()
        {
            var daysOfContainer = new List<Day?>();
            this.GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            daysOfContainer.AddRange(_month.Days);
            this.GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            return daysOfContainer;
        }
        
        public void Next()
        {
            var nextDateTime = _month.MonthDateTime.AddMonths(1);
            _month = new Month(nextDateTime);
            _days = null;
        }

        public void Previous()
        {
            var previousDateTime = _month.MonthDateTime.AddMonths(-1);
            _month = new Month(previousDateTime);
            _days = null;
        }
    }
}
