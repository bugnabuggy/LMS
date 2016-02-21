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

        public DateTime Timestamp { get; set; }

        public string AreaId { get; set; }

        public UserArea Area { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public GoalStateType StateId { get; set; }

        public string State { get; set; }

        public GoalVM()
        {
        }
    }
}
