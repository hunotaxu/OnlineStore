using DAL.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class Address : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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