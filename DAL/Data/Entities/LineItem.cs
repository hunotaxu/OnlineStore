using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models;

namespace DAL.Data.Entities
{
    public class LineItem
    {
        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName="decimal(18,2)")]
        public decimal Amount { get; set; }

        public decimal SaleOff { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
