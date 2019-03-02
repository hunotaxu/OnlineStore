using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
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

        public ICollection<Item> Item { get; set; }
    }
}
