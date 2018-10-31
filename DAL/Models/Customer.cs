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
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int Status { get; set; }

        [MaxLength(500)]
        public string ShippingAddress { get; set; }

        [MaxLength(500)]
        public string BillingAddress { get; set; }

        public int TypeOfCustomerId { get; set; }

        public Gender Gender { get; set; }
        public TypeOfCustomer TypeOfCustomer { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}