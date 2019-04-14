using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class CartDetail
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Item Item { get; set; }
    }
}
