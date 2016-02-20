using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class UserArea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(ModelConstrains.IdLength)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(ModelConstrains.IdLength)]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        [MaxLength(ModelConstrains.TitleLength)]
        public string Title { get; set; }

        [StringLength(ModelConstrains.ColorLength)]
        public string Color { get; set; }

        public int Priority { get; set; }

        public UserArea()
        {
            Id = IdFactory.GenerateId();
            Timestamp = DateTime.UtcNow;
        }
    }
}
