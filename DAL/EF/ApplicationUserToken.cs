using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}