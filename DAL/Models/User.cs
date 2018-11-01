using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class User : EntityBase
    {
        //public int Id { get; set; }
        [Display(Name="Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        public int TypeOfUserId { get; set; }

        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Status { get; set; }
        //public byte[] Timestamp { get; set; }

        public TypeOfUser TypeOfUser { get; set; }
        public Gender Gender { get; set; }
        //public ICollection<Cart> Cart { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }
}
