using System;
using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    // Add profile data for application users by adding properties to the OnlineStoreUser class
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
