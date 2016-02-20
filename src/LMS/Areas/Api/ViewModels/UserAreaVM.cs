using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Core.Models;

namespace LMS.Areas.Api.ViewModels
{
    public class UserAreaVM
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstrains.TitleLength)]
        public string Title { get; set; }

        [StringLength(ModelConstrains.ColorLength)]
        public string Color { get; set; }

        public int Priority { get; set; }

        public List<GoalVM> Goals { get; set; }

        public UserAreaVM()
        {
            Goals = new List<GoalVM>();
        }
    }
}
