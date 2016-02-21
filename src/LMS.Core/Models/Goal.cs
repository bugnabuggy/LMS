using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class Goal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(ModelConstrains.IdLength)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(ModelConstrains.IdLength)]
        public string AreaId { get; set; }

        public UserArea Area { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public GoalStateType StateId { get; set; }

        public GoalState State { get; set; }

        public List<CalendarTask> Tasks { get; set; }

        public int Priority { get; set; }

        public Goal()
        {
            Id = IdFactory.GenerateId();
            Timestamp = DateTime.UtcNow;
            Tasks = new List<CalendarTask>();
        }
    }
}
