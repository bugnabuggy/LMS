using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class AppContext : IAppContext
    {
        public string UserId { get; set; }

        public string TimeZoneId { get; set; }
    }
}
