using DAL.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Entities
{
    public class UserAddress
    {
        public Guid CustomerId { get; set; }
        public int AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string RecipientName { get;set; }
        public bool IsDeleted { get; set; }

        [Timestamp]
        public virtual ApplicationUser Customer { get; set; }
        public virtual Address Address { get; set; }
    }
}
