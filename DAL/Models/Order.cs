using DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Order : EntityBase
    {
        public Order()
        {
            LineItems = new HashSet<LineItem>();
        }

        //public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }

        public int EmployeeId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        public StatusOrder Status { get; set; }
        //public byte[] Timestamp { get; set; }

        public Customer Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }
}
