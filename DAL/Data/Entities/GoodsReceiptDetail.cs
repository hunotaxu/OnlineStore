using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.Data.Entities
{
    public class GoodsReceiptDetail
    {
        public int GoodsReceiptId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public int ItemId { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public bool IsDeleted { get; set; }

        public virtual GoodsReceipt GoodsReceipt { get; set; }
        public virtual Item Item { get; set; }
    }
}
