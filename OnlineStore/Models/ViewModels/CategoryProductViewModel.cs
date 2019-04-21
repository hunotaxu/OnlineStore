using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class CategoryProductViewModel
    {
        [Column(TypeName = "nvarchar(200)")]
        [Display(Name = "Tên loại")]
        [Required]
        public string Name { get; set; }

        //public byte[] Timestamp { get; set; }

        public ICollection<Item> Item { get; set; }


    }
}
