using DAL.Data.Entities.Base;
using DAL.Models;
using System;

namespace DAL.Data.Entities
{
    public class Address : EntityBase
    {
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Detail { get; set; }
        public string PhoneNumber { get; set; }
        public string RecipientName { get; set; }
        public Guid CustomerId { get; set; }
        public int? ShowRoomAddressId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual DefaultAddress DefaultAddress { get; set; }
        public virtual ShowRoomAddress ShowRoomAddress { get; set; }
    }
}