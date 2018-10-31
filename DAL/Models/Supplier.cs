using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Supplier : EntityBase
    {
        public Supplier()
        {
            GoodsReceipts = new HashSet<GoodsReceipt>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<GoodsReceipt> GoodsReceipts { get; set; }
    }
}
