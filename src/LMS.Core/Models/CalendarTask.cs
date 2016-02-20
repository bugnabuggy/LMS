using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class CalendarTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(ModelConstrains.IdLength)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(ModelConstrains.IdLength)]
        public string GoalId { get; set; }

        public Goal Goal { get; set; }

        public int TimeSpendMin { get; set; }
    }
}
