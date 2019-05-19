using System;
using System.Collections.Generic;
using DAL.Data.Enums;
using DAL.Data.Interfaces;
using DAL.EF;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationRole : IdentityRole<Guid>, IDateTracking, ISwitchable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public byte Status { get; set; }

        public string Description { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
