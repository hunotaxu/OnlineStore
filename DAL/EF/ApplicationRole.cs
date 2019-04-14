using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
