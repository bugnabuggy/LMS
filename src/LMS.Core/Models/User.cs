using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(256)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public User()
        {
            Id = IdFactory.GenerateId();
            Timestamp = DateTime.UtcNow;
        }
    }
}
