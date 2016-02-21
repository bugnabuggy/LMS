using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class TimeConverter : ITimeConverter
    {
        private IAppContext _appContext;

        private readonly TimeZoneInfo _localTimeZone;

        public TimeConverter(IAppContext appContext)
        {
            _appContext = appContext;
            _localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_appContext.TimeZoneId);
        }

        public DateTime ToLocal(DateTime utcTime)
        {
            var time = new DateTime(utcTime.Ticks, DateTimeKind.Utc);
            var localTime = TimeZoneInfo.ConvertTime(time, _localTimeZone);
            return localTime;
        }

        public DateTime ToUtc(DateTime localTime)
        {
            var time = new DateTime(localTime.Ticks, DateTimeKind.Unspecified);
            var utcTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Utc);
            return utcTime;
        }
    }
}
