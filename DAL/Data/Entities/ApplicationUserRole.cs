using System;
using DAL.Data.Enums;
using DAL.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data.Entities
{
    public class ApplicationUserRole : IdentityUserRole<Guid>, IDateTracking, ISwitchable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public byte Status { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
