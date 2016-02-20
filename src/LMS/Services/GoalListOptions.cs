using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class GoalListOptions
    {
        public string AreaId { get; set; }

        public bool OnlyLastGoals { get; set; }

        public bool IncludeCompletedGoals { get; set; }
    }
}
