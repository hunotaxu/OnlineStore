using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;
using DAL.Data.Enums;
using Newtonsoft.Json;

namespace DAL.Data.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
            LineItems = new HashSet<OrderItem>();
        }

        [Display(Name = "Ngày giao dự kiến")]
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        //public decimal? Bonus { get; set; }

        [Display(Name = "Người duyệt")]
        public Guid? EmployeeId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public Guid CustomerId { get; set; }

        public int? AddressId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Display(Name = "Trạng thái")]
        public OrderStatus Status { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public decimal SaleOff { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser Customer { get; set; }
        [JsonIgnore]
        public virtual Address Address { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderItem> LineItems { get; set; }
    }
}
