using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class GoodsReceipt : EntityBase
    {
        public GoodsReceipt()
        {
            GoodsReceiptDetail = new HashSet<GoodsReceiptDetail>();
        }

        //public int Id { get; set; }
        public int? SupplierId { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? EmployeeId { get; set; }
        //public byte[] Timestamp { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetail { get; set; }
    }
}
