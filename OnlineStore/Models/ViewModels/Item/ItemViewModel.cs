using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DAL.Data.Enums;

namespace OnlineStore.Models.ViewModels.Item
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int Inventory { get; set; }

        public decimal? AverageEvaluation { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? View { get; set; }

        public string BrandName { get; set; }

        public CategoryViewModel Category { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public byte? Status { set; get; }
    }
}
