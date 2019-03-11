using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;
using DAL.Data.Enums;
using DAL.Models;

namespace DAL.Data.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
            LineItems = new HashSet<LineItem>();
        }

        //public int Id { get; set; }
        [Display(Name = "Ngày giao dự kiến")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }

        [Display(Name = "Người duyệt")]
        public int EmployeeId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public int CustomerId { get; set; }

        public int AddressId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Display(Name = "Trạng thái")]
        public StatusOrder Status { get; set; }
        //public byte[] Timestamp { get; set; }

        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }
}
