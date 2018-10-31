using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Item : EntityBase
    {
        public Item()
        {
            CartDetails = new HashSet<CartDetail>();
            Comments = new HashSet<Comment>();
            GoodsReceiptDetails = new HashSet<GoodsReceiptDetail>();
            LineItems = new HashSet<LineItem>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Inventory { get; set; }
        public int View { get; set; }
        public int CategoryId { get; set; }
        public bool Deleted { get; set; }
        public decimal AverageEvaluation { get; set; }
        public int EventId { get; set; }
        //public byte[] Timestamp { get; set; }

        public Category Category { get; set; }
        public Event Event { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }
}
