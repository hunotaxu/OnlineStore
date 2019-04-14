using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Data.Entities.Base;
using DAL.Data.Enums;
using DAL.Models;

namespace DAL.Data.Entities
{
    public class Customer : EntityBase
    {
        public Customer()
        {
            Cart = new Cart();
            Orders = new HashSet<Order>();
            Comments = new HashSet<Comment>();
			Addresses = new HashSet<Address>();
        }

        [Required(ErrorMessage = "Họ tên là bắt buộc. Vui lòng nhập")]
        [MaxLength(100)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        //[MaxLength(100)]
        //[Display(Name = "Họ")]
        //public string LastName { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc. Vui lòng nhập")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.Vui lòng nhập")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.Vui lòng nhập")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        //[Required]
        //public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.Vui lòng nhập")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required]
        public int Status { get; set; }

        [MaxLength(500)]
        public string ShippingAddress { get; set; }

        [MaxLength(500)]
        public string BillingAddress { get; set; }

        public int TypeOfCustomerId { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc.Vui lòng chọn")]
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        public virtual TypeOfCustomer TypeOfCustomer { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}