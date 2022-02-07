using DAL.Data.Entities.Base;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Entities
{
    public class GoodsReceiptDetail : EntityBase
    {
        public int GoodsReceiptId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public int ItemId { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        public virtual GoodsReceipt GoodsReceipt { get; set; }
        public virtual Item Item { get; set; }
    }
}