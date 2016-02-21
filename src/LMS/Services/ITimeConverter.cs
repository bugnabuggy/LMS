using System;

namespace LMS.Services
{
    public interface ITimeConverter
    {
        DateTime ToLocal(DateTime utcTime);
        DateTime ToUtc(DateTime localTime);
    }
}