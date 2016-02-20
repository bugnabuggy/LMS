using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class GoalState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public GoalStateType Id { get; set; }

        [StringLength(ModelConstrains.TitleLength)]
        public string Title { get; set; }
    }
}
