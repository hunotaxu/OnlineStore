using DAL.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class OrderItem : EntityBase
    {
        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public decimal Price { get; set; }

        public decimal? SaleOff { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}