using DAL.Data.Entities.Base;
using DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class Supplier : EntityBase
    {
        public Supplier()
        {
            GoodsReceipts = new HashSet<GoodsReceipt>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; }
    }
}