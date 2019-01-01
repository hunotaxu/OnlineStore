using System.ComponentModel;
using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class User : EntityBase
    {
        //public int Id { get; set; }
        [Display(Name="Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Địa chỉ")]
        [Column(TypeName = "nvarchar(500)")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int TypeOfUserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Trang thái")]
        public int Status { get; set; }
        //public byte[] Timestamp { get; set; }

        [Display(Name = "Loại người dùng")]
        public TypeOfUser TypeOfUser { get; set; }

        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }
        //public ICollection<Cart> Cart { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }
}
