using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierId { get; set; }

        [Display(Name = "Tổng giá trị")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
    }
}
