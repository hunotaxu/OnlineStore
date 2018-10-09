using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class GoodsReceipt
    {
        public GoodsReceipt()
        {
            GoodsReceiptDetail = new HashSet<GoodsReceiptDetail>();
        }

        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? EmployeeId { get; set; }
        public byte[] Timestamp { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetail { get; set; }
    }
}
