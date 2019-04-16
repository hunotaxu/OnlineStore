﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Category : EntityBase
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }

        //public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        [Display(Name="Tên loại")]
        [Required]
        public string Name { get; set; }

        //public byte[] Timestamp { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}