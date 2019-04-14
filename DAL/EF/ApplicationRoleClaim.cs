using Microsoft.AspNetCore.Identity;

namespace DAL.EF
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}