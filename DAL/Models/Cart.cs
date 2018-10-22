using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Cart : EntityBase
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
