using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Supplier : EntityBase
    {
        public Supplier()
        {
            GoodsReceipts = new HashSet<GoodsReceipt>();
        }

        //public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        public ICollection<GoodsReceipt> GoodsReceipts { get; set; }
    }
}
