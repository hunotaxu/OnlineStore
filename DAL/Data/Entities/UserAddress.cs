using System;

namespace DAL.Data.Entities
{
    public class UserAddress
    {
        public Guid CustomerId { get; set; }
        public int AddressId { get; set; }
        public bool IsDeleted { get; set; }
        public byte AddressType { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual Address Address { get; set; }
    }
}
