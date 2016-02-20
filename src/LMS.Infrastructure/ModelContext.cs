using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models;
using Microsoft.Data.Entity;

namespace LMS.Infrastructure
{
    public class ModelContext : DbContext
    {
        public DbSet<Goal> Goals { get; set; }

        public DbSet<GoalState> GoalStates { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserArea> UserAreas { get; set; }

        public DbSet<CalendarTask> CalendarTasks { get; set; }
    }
}
