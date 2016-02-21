using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Areas.Api.ViewModels
{
    public class CalendarTaskVM
    {
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        [Required]
        public int TimeSpentMin { get; set; }
    }
}
