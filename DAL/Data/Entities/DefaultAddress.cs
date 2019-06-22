using System;

namespace DAL.Data.Entities
{
    public class DefaultAddress
    {
        public int AddressId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual Address Address { get; set; }
    }
}