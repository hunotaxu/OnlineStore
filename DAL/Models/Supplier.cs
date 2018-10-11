using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Supplier : EntityBase
    {
        public Supplier()
        {
            GoodsReceipt = new HashSet<GoodsReceipt>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<GoodsReceipt> GoodsReceipt { get; set; }
    }
}
