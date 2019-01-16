using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class LineItem
    {
        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public bool Deleted { get; set; }

        [Column(TypeName="decimal(18,2)")]
        public decimal Amount { get; set; }

        public Item Item { get; set; }
        public Order Order { get; set; }
    }
}
