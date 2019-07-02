using System;
using System.ComponentModel.DataAnnotations;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Comment : EntityBase
    {
        //public int Id { get; set; }
        public string Content { get; set; }

        public int ItemId { get; set; }

        [Range(1, 5)]
        public int Evaluation { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual Item Item { get; set; }

    }
}
