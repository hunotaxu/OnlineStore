using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Entities
{
    public class CartDetail
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public bool Deleted { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Cart Cart { get; set; }
        public Item Item { get; set; }
    }
}
