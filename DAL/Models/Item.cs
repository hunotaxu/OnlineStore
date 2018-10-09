using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Item
    {
        public Item()
        {
            CartDetail = new HashSet<CartDetail>();
            Comment = new HashSet<Comment>();
            GoodsReceiptDetail = new HashSet<GoodsReceiptDetail>();
            LineItem = new HashSet<LineItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int? Inventory { get; set; }
        public int? View { get; set; }
        public int? CategoryId { get; set; }
        public bool? Deleted { get; set; }
        public decimal? AverageEvaluation { get; set; }
        public int? EventId { get; set; }
        public byte[] Timestamp { get; set; }

        public Category Category { get; set; }
        public Event Event { get; set; }
        public ICollection<CartDetail> CartDetail { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetail { get; set; }
        public ICollection<LineItem> LineItem { get; set; }
    }
}
