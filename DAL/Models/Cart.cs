using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Cart : EntityBase
    {
        public Cart()
        {
            CartDetail = new HashSet<CartDetail>();
        }

        //public int Id { get; set; }
        public int CustomerId { get; set; }
        //public byte[] Timestamp { get; set; }

        public User Customer { get; set; }
        public ICollection<CartDetail> CartDetail { get; set; }
    }
}
