using DAL.Data.Entities.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DAL.Data.Entities
{
    public class ReceivingType : EntityBase
    {
        public ReceivingType()
        {
            Orders = new HashSet<Order>();
        }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
