using DAL.Data.Entities.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class ReceivingType : EntityBase
    {
        public ReceivingType()
        {
            Orders = new HashSet<Order>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Value { get; set; }
        public int NumberShipDay { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}