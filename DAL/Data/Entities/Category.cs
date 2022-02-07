using DAL.Data.Entities.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class Category : EntityBase
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [Display(Name = "Tên loại")]
        [Required]
        public string Name { get; set; }

        public byte? SortOrder { set; get; }

        public int? ParentId { set; get; }

        public byte? Status { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        //public virtual ICollection<Category> Parent { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}