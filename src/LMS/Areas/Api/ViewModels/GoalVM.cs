using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models;

namespace LMS.Areas.Api.ViewModels
{
    public class GoalVM
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public GoalStateType StateId { get; set; }

        public int Priority { get; set; }

        public GoalVM()
        {
        }
    }
}
