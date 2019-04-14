using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
