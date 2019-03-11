using System.Collections.Generic;
using DAL.Data.Entities.Base;
using DAL.Models;

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

        public Customer Customer { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}