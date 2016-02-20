using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace LMS.Infrastructure
{
    public class ModelContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Goal> Goals { get; set; }

        public DbSet<GoalState> GoalStates { get; set; }

        public DbSet<UserArea> UserAreas { get; set; }

        public DbSet<CalendarTask> CalendarTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
