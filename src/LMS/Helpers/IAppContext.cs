using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public interface IAppContext
    {
        string UserId { get; }

        string TimeZoneId { get; }
    }
}
