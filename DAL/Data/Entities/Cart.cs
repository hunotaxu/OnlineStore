using System.Collections.Generic;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Cart : EntityBase
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        //public int Id { get; set; }
        public int CustomerId { get; set; }
        //public byte[] Timestamp { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}