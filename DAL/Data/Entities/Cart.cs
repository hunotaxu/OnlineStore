using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Cart : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid CustomerId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}