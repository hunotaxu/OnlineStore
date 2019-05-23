using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Order : EntityBase
    {
        public Order()
        {
            LineItems = new HashSet<LineItem>();
        }

        [Display(Name = "Ngày giao dự kiến")]
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Bonus { get; set; }

        [Display(Name = "Người duyệt")]
        public Guid? EmployeeId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public Guid CustomerId { get; set; }

        public int? AddressId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ShippingFee { get; set; }

        [Display(Name = "Trạng thái")]
        public byte Status { get; set; }

        public byte DeliveryType { get; set; }

        public byte PaymentType { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<LineItem> LineItems { get; set; }
    }
}
