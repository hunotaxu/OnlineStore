using System;
using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    // Add profile data for application users by adding properties to the OnlineStoreUser class
    public class OnlineStoreUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}
