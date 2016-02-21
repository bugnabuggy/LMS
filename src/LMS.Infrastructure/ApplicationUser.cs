using System.ComponentModel.DataAnnotations;
using LMS.Core;
using LMS.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser()
        {
            User = new User
            {
                Id = this.Id
            };
            UserId = User.Id;
        }
    }
}
