using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class CartDetail
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Cart Cart { get; set; }
        public Item Item { get; set; }
    }
}
