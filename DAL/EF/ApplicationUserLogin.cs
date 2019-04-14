using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}