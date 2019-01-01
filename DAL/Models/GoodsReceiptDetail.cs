using System.ComponentModel.DataAnnotations;

namespace DAL.Models
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

        public GoodsReceipt GoodsReceipt { get; set; }
        public Item Item { get; set; }
    }
}
