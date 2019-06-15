using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Enums;
using DAL.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data.Entities
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
        [Required(ErrorMessage = "Giới tính là bắt buộc.Vui lòng chọn")]
        [Display(Name = "Giới tính")]
        public byte Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
