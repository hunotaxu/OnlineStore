using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class GoodsReceipt : EntityBase
    {
        public GoodsReceipt()
        {
            GoodsReceiptDetails = new HashSet<GoodsReceiptDetail>();
        }

        //public int Id { get; set; }
        public int SupplierId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
    }
}
