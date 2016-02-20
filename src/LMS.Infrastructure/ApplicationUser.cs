using LMS.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public User User { get; set; }

        public string UserId { get; set; }
    }
}
