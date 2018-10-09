using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class CartDetail
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
        public byte[] Timestamp { get; set; }

        public Cart Cart { get; set; }
        public Item Item { get; set; }
    }
}
