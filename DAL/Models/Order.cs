using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Order : EntityBase
    {
        public Order()
        {
            LineItems = new HashSet<LineItem>();
        }

        //public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Bonus { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public decimal ShippingFee { get; set; }
        public int Status { get; set; }
        //public byte[] Timestamp { get; set; }

        public Customer Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }
}
