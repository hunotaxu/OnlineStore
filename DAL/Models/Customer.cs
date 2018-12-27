using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Base;

namespace DAL.Models
{
    public class Customer : EntityBase
    {
        public Customer()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Comments = new HashSet<Comment>();
        }

        [MaxLength(100)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

    

        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
      

        [Display(Name = "Mật khẩu")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Trạng thái")]
        [Required]
        public int Status { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [MaxLength(500)]
        public string ShippingAddress { get; set; }

        [Display(Name = "Địa chỉ thanh toán")]
        [MaxLength(500)]
        public string BillingAddress { get; set; }

        [Display(Name = "Mã loại khách hàng")]
        public int TypeOfCustomerId { get; set; }

        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Display(Name = "Loại KH")]
        public TypeOfCustomer TypeOfCustomer { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}